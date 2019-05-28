# Schematics

## Features

### Core features

* Define CRUD functionality for Asp.Net Core app with consistent generic API
* Entities Metadata
* Entities Versioning
* Access rules
* Storage agnostic (plugins for specific storage technologies)
* Define Entities at startup with generic or custom read \ write logic
* Feature-based "plugin" system for storage (system defines capabilities which may or may not be implemented by specific storage plugin)

### Advanced features

* Metadata localization
* Create entities at runtime
* GraphQL

## Model

* Entity
* Property
* Instance
* Property Value
* Type
    * Scalar
    * Reference
    * Collection
    * Union
* Feature
* Source

## Assumptions, Caveats and Opinions

* Entity MUST have zero or one key.
