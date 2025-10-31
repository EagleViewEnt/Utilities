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

using System.Globalization;
using System.Reflection;

using AKSoftware.Localization.MultiLanguages;
using AKSoftware.Localization.MultiLanguages.Providers;

namespace MgbUtilties.Core.Extensions.Language
{

    public class MgbKeysProvider : IKeysProvider
    {

        readonly Dictionary<string, string> _languages = [];
        readonly string _resourcesFolderName = "Resources";

        public MgbKeysProvider( string resourcesFolderName = "Resources" )
        {
            _resourcesFolderName = resourcesFolderName;
            GetResources();
        }

        public IEnumerable<CultureInfo> RegisteredLanguages => _languages.Keys.Select(name => new CultureInfo(name));

        protected static string GetAssemblyKeys( Assembly assembly
                                                , string resourcePath )
        {
            if(string.IsNullOrWhiteSpace(resourcePath))
                throw new ArgumentNullException(nameof(resourcePath));

            try {
                using Stream stream = assembly.GetManifestResourceStream(resourcePath)!;
                using StreamReader streamReader = new(stream);
                return streamReader.ReadToEnd();
            } catch(Exception) {
                return string.Empty;
            }
        }

        public static string GetCultureNameFromPath( string namespacePath )
        {
            string[] resourceParts = namespacePath.Split('.');
            return resourceParts[resourceParts.Length - 2];
        }

        public Keys GetKeys( CultureInfo cultureInfo ) => GetKeys(cultureInfo.Name);

        public Keys GetKeys( string cultureName ) => new(_languages[cultureName]);

        string[] GetLanguageFileNames( Assembly assembly )
            => [.. from s in assembly.GetManifestResourceNames()
            where s.Contains(_resourcesFolderName) && (s.Contains(".yml") || s.Contains(".yaml")) && s.Contains('-')
            select s];

        /// <summary>
        /// Method looks through all loaded assemblies for Yml resources. If found they are added to  a dictionary by
        /// language.
        /// </summary>
        protected void GetResources()
        {
            Assembly[] assembilies = AppDomain.CurrentDomain.GetAssemblies();
            foreach(Assembly assembly in assembilies) {
                if(!HasResources(assembly))
                    continue;

                string[] resourcePaths = GetLanguageFileNames(assembly);
                foreach(string path in resourcePaths) {
                    string resources = GetAssemblyKeys(assembly, path);

                    string cultureName = GetCultureNameFromPath(path);

                    if(!_languages.Any(a => string.Compare(a.Key, cultureName, StringComparison.Ordinal) == 0))
                        _languages.Add(cultureName, resources);
                    else
                        _languages[cultureName] += $"{Environment.NewLine}{resources}";
                }
            }
        }

        bool HasResources( Assembly assembly )
            => assembly.GetManifestResourceNames()
                       .Any(a => a.Contains(_resourcesFolderName) && (a.Contains(".yml") || a.Contains(".yaml")));

    }

}
