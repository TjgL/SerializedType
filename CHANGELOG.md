# Changelog

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [0.1.1] - 2025-08-09
### Added
* The static method `FromType<>()` to explicitly convert any type to a `SerializedType`.
* An implicit conversion operator from `System.Type` to `SerializedType`.
    * This method is not recommended because it doesn't cause a compile time error, only a runtime exception.
    * This is only available under the new Scripting Define `ST_ALLOW_IMPLICIT_CASTS`.

### Changed
* Updated readme with usage examples.

## [0.1.0] - 2025-08-08
- Initial Release