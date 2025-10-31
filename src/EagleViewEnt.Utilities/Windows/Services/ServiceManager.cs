//-----------------------------------------------------------------------
// <copyright 
//	   Author="Brian Dick"
//     Company="Eagle View Enterprises LLC"
//     Copyright="(c) Eagle View Enterprises LLC. All rights reserved."
//     Email="support@eagleviewent.com"
//     Website="www.eagleviewent.com"
// >
//	   <Disclaimer>
//			THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
// 			TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
// 			THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
// 			CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// 			DEALINGS IN THE SOFTWARE.
// 		</Disclaimer>
// </copyright>
//-----------------------------------------------------------------------

using System.Diagnostics;
using System.Runtime.Versioning;
using System.ServiceProcess;

using MgbUtilties.Core.Extensions.Logger;

using Serilog;

namespace MgbUtilities.Windows.Services;

/// <summary>
///  Provides methods to manage and control system processes and services.
/// </summary>
/// <remarks>
///  The <see cref="ServiceManager" /> class offers functionality to ensure a single instance of a process is running
///  and to recycle system services by stopping and starting them. It handles errors by logging them and provides
///  feedback on the success of operations.
/// </remarks>
[SupportedOSPlatform("windows")]
public class ServiceManager
{

    /// <summary>
    ///  Ensures that only a single instance of the specified process is running by terminating any other instances.
    /// </summary>
    /// <remarks>
    ///  This method attempts to terminate all other processes with the same name as the specified <paramref
    ///  name="currentProcess" />. If an error occurs during this operation, it logs the error and returns <see
    ///  langword="false" />.
    /// </remarks>
    /// <param name="currentProcess">The current process instance that should remain running.</param>
    /// <returns>
    ///  <see langword="true" /> if all other instances were successfully terminated; otherwise, <see langword="false"
    ///  />.
    /// </returns>
    public static bool ForceSingleInstance( Process currentProcess )
    {
        try {
            Process[] processes = Process.GetProcessesByName(currentProcess.ProcessName);
            foreach(Process process in processes)
                if(process.Id != currentProcess.Id)
                    process.Kill();
            return true;
        } catch(Exception ex) {
            Log
                .Logger
            .WithCallingContext<ServiceManager>()
            .Error(ex, "Fail to force single instance.");
            return false;
        }
    }

    /// <summary>
    ///  Recycles the specified services by stopping and then starting each one.
    /// </summary>
    /// <remarks>
    ///  This method attempts to stop each service if it is running and then start it again. If any service fails to
    ///  recycle, the method logs the error and continues with the next service.
    /// </remarks>
    /// <param name="services">An array of service names to be recycled. Each service will be stopped if running and then started.</param>
    /// <returns>
    ///  <see langword="true" /> if all specified services were successfully recycled; otherwise, <see langword="false"
    ///  />.
    /// </returns>
    public static bool RecycleServices( params string[] services )
    {
        bool result = true;
        foreach(string serviceName in services)
            try {
                using ServiceController service = new ServiceController(serviceName);
                Log
                    .Logger
                .Debug("Recycling {ServiceName}...", serviceName);

                // Stop the service if it's running
                if(service.Status != ServiceControllerStatus.Stopped) {
                    Log
                        .Logger
                    .Debug("Stopping {ServiceName}...", serviceName);

                    service.Stop();
                    service.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(30));
                }

                // Start the service
                Log
                    .Logger
                .Debug("Starting {ServiceName}...", serviceName);
                service.Start();
                service.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(30));

                Log
                    .Logger
                .Debug("Service {ServiceName} started.", serviceName);
            } catch(Exception ex) {
                result = false;
                Log
                    .Logger
                .Error(ex, "Service {ServiceName} failed to recycle", serviceName);
            }
        return result;
    }

}
