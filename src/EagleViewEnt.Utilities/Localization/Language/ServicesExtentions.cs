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

using AKSoftware.Localization.MultiLanguages;
using AKSoftware.Localization.MultiLanguages.Providers;
using Microsoft.Extensions.DependencyInjection;
using MgbUtilties.Core.Extensions.Language;

namespace MgbUtilties.Localization.Language;

public static partial class LanguageExtensions
{

    public static IServiceCollection AddMgbLanguageServices( this IServiceCollection services )
    {
        services.AddSingleton<IKeysProvider, MgbKeysProvider>(( s ) => new MgbKeysProvider());
        return services.AddSingleton<ILanguageContainerService, LanguageContainerInAssembly>(( s )
            => new LanguageContainerInAssembly(s.GetService<IKeysProvider>()));
    }

}
