# UpmImporter

Some utility scripts to allow install UPM packages on package import / Unity launch.

## API Documentation

Documentation available at https://poi-vrc.github.io/UpmImporter

## Creating an installer

Please follow this convention when you are trying to creating an installer using UpmImporter.

You can avoid using this convention and place the file in your own folder if you do not need to block
installations if the old installation places scripts in `Assets`, and require users to remove those
files manually.

```
Assets
|-> chocopoi
    |-> UpmImporter
        |-> Editor
            |-> Installers
                |-> Example_Package_Installer.cs.template
                |-> Com_Chocopoi_DressingTools_Installer.cs
                |-> {YOUR-PACKAGE-NAME-HERE}_Installer.cs
```

The file `Example_Package_Installer.cs.template` contains a template for you to customize for your own setup.

The code will self-delete after its first execution. Therefore, it allows developers to create an 
Unity package to install an UPM package from the registry automatically.