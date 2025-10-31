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

using System;
using System.Linq;

using MgbUtilties.Media.Images.Enums;

namespace MgbUtilties.Media.Images
{

    public record MgbImageStyles
        (MgbImagePosition Position = MgbImagePosition.inherit
          , int MinHeight = 0
          , int MaxHeight = 0
          , int MinWidth = 0
          , int MaxWidth = 0
          , int Width = 0
          , int Height = 375
          , int Top = 0
          , int Left = 0
          , int Bottom = 0
          , int Right = 0
          , int Zindex = 0
          , bool UseImageStyles = true )
    {
        public override string ToString()
        {
            string result = string.Empty;
            result += (MaxHeight != 0) ? ($"max-height:{MaxHeight}px; ") : string.Empty;
            result += (MinHeight != 0) ? ($"min-height:{MinHeight}px; ") : string.Empty;
            result += (MaxWidth != 0) ? ($"max-width:{MaxWidth}px; ") : string.Empty;
            result += (MinWidth != 0) ? ($"min-width:{MinWidth}px; ") : string.Empty;
            result += (Position == MgbImagePosition.absolute) ? ($"top:{Top}px; ") : string.Empty;
            result += (Position == MgbImagePosition.absolute) ? ($"left:{Left}px; ") : string.Empty;
            result += (Position == MgbImagePosition.absolute) ? ($"bottom:{Bottom}px; ") : string.Empty;
            result += (Position == MgbImagePosition.absolute) ? ($"right:{Right}px; ") : string.Empty;
            result += $"z-index:{Zindex}; ";
            result += (Position != MgbImagePosition.inherit)
                ? ($"position:{Enum.GetName(typeof(MgbImagePosition), Position)};")
                : string.Empty;

            return result.Trim();
        }
    }

}

