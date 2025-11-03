# EagleViewEnt.Utilities.Core

Core utilities for EagleViewEnt apps (extensions, types, guards). Target framework: .NET 9 (net9.0).

- NuGet PackageId: `EagleViewEnt.Utilities.Core`
- Description: Core utilities for EagleViewEnt apps (extensions, types, guards).

## Install

- PackageReference:
  <PackageReference Include="EagleViewEnt.Utilities.Core" />
- CLI:
  dotnet add package EagleViewEnt.Utilities.Core

## Extensions

- EagleViewEnt.Utilities.Core.Extensions.String.CasingExtensions
  - Provides string casing-related extension methods, including camel-/Pascal-case splitting and proper-case conversion.

- EagleViewEnt.Utilities.Core.Extensions.String.HtmlExtensions
  - Extension methods for working with HTML content in strings. Provides helpers to convert a string to a Blazor MarkupString and to remove HTML tags.

- EagleViewEnt.Utilities.Core.Extensions.String.MaskingExtensions
  - Provides string extension methods for masking and extracting characters.
  - Example:
    - "123456789".ToSecurityMaskedString() => "****6789" (default: last 4 visible, total length 8)

- EagleViewEnt.Utilities.Core.Extensions.String.NumericExtensions
  - Provides extension methods for numeric values related to string formatting.
  - Example:
    - 21.ToStringWithOrdinalIndicator() => "21st"
    - 21.ToStringWithOrdinalIndicator(asHtml: true) => "21<sup>st</sup>"

- EagleViewEnt.Utilities.Core.Extensions.String.ValidationExtensions
  - Provides string validation extension methods for common null/empty checks and guard clauses.

- EagleViewEnt.Utilities.Core.Extensions.Numeric.DecimalExtensions
  - Provides extension methods for decimal values, including banker’s rounding, currency formatting, fractional string conversion, and rounding away from zero.

- EagleViewEnt.Utilities.Core.Extensions.Navigation.NavigationManagerExtensions
  - Provides extension methods for NavigationManager to assist with query string parameter retrieval.

## Mapping

- EagleViewEnt.Utilities.Core.Mapping.AutoMap
  - Provides methods for deep copying and mapping objects and collections using reflection.

## Value Types (string-backed)

- EagleViewEnt.Utilities.Core.Types.ValueTypes.String.StringValueType<TSelf>
  - Strongly-typed, validated string value object base using CRTP.
  - Key members:
    - IsEmpty: indicates empty value.
    - ToSecuredString(int visibleLength = 4, int totalLength = 8, char padCharacter = '*'): masked display using masking extensions.
    - Validation(): abstract; implemented by derived types.

## Checks

- EagleViewEnt.Utilities.Core.Types.Checks.AccountNumber
  - Immutable value type for AccountNumber fields. Provides masked display helpers via ToSecuredString/ToSecurityMaskedString and AsAccountNumberSecured().

- EagleViewEnt.Utilities.Core.Types.Checks.RoutingNumber
  - Represents an ABA routing transit number (RTN). Validates 9 digits and ABA check digit (7-3-9 weighting).

- EagleViewEnt.Utilities.Core.Types.Checks.CheckNumber
  - Represents a validated check number value object backed by a string. Empty allowed; non-empty must be numeric and fewer than 6 chars.

- EagleViewEnt.Utilities.Core.Types.Checks.Micr
  - Strongly-typed representation of a bank MICR line, extracting routing, account, and optional check numbers.
  - Format: "@{routing9}@{account6-17}[+{check0-10}]".

## File Paths

- EagleViewEnt.Utilities.Core.Types.FilePath.EveFilePath
  - Strongly-typed file path string with validation and file system checks. Normalized to lowercase for case-insensitive equality and hashing.

## XML Helpers

- EagleViewEnt.Utilities.Core.Types.XmlString.EveXml
  - Immutable XML string value with helpers for validation and conversions. Input is trimmed; exposes IsValid.

- EagleViewEnt.Utilities.Core.Types.XmlString.Extensions.EveXmlExtensions
  - Extension helpers for EveXml and XML serialization, including converting objects to XML and deserializing from XML strings or files.

- EagleViewEnt.Utilities.Core.Types.ValueTypes.String.Converters.StringValueTypeXmlConverter<T>
  - XML-serialization helper for string-backed value types implementing IStringValueType.

## Quick start

- Masking
  - "123456789".ToSecurityMaskedString() // ****6789

- AccountNumber
  - AccountNumber acct = "123456789";
  - string masked = acct.ToSecuredString(); // ****6789

- RoutingNumber validation
  - RoutingNumber rn = "123456780";
  - rn.ToString(); // raw value; Validation() enforces checksum

- MICR parsing
  - Micr micr = "@123456789@123456789012+1001";
  - micr.RoutingNumber, micr.AccountNumber, micr.CheckNumber

- XML
  - var xml = EveXmlExtensions.ToXml(new { Name = "Test" });
  - var obj = xml.As<YourType>();

## Build

- Target framework: net9.0
- Open in Visual Studio and build. README is packed with the NuGet package when present.

## License

See repository for license details.

