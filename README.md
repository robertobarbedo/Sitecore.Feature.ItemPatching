# Sitecore.Feature.ItemPatching

------------------------------------------------------------------------------------------------------
See https://www.getfishtank.com/insights/sitecore-environment-specific-items-introducing-item-patching
------------------------------------------------------------------------------------------------------



## A Module for Environment-Specific Sitecore Item Management
A module that enables developers to define environment-specific values within Sitecore items. The module will automatically apply changes based on predefined configurations, ensuring a seamless data restore experience.

## Key Features of the Environment-Specific Item Patching Module
* Environment-Aware Item Storage: Store environment-specific values alongside regular Sitecore items without interfering with Sitecore functioning.
* Automatic Application of Values: The module automatically updates items based on the environment when a package is installed or the server restarts.
* Simple Configuration: Developers can configure which items and environments are managed using a simple .config file.

## How the Module Works to Manage Sitecore Item Variations per Environment
You can make any item in the content tree to be subject of environment-specific values. In the image below, we have some fields from “Settings” differing across environments.
* The module automatically generates additional environment-specific items (represented in gray in the image).
* Each environment-specific item stores only the difference between the main “Settings” item.
* The values can be applied to the main item based on events such as Initialize and Package Installed.

## Getting Started with the Sitecore Environment-Specific Item Patching Module
### Download the module.
### Add it to your solution.
### Configure.
* Edit the ItemPatching.z.MyClient.config file.
* Define the environments used in your organization.
* Specify the locations and templates that will be controlled.

It is plug-and-play - once deployed, the module automatically generates buttons, templates, and everything it needs to function.

## Configuration Settings for the Sitecore Item Patching Module
All project specific configuration can be done in App_Config/Include/ItemPatching.z.MyClient.config - there are pre-defined values to help understanding, they need to be replaced by your own project settings.
For Sitecore XM/XP you will need <add key="env:define" value="[dev,qa,etc]" /> in the web.config.
For XM Cloud you will need a project variable in your CM: $(env:ENVIRONMENT_NAME).

## How to Use the Module Within the Sitecore Content Editor
Once configured and deployed to your solution, the module provides two new buttons under the Developer

### Generate
* Creates all necessary items for configuring environment-specific values.
* These items should be serialized and pushed to source control if its original item is also serialized.
* Use generate only when you need to create a new item to generate environment gray items as children.
### Apply
* Transfers values by copying the fields from the environment-specific item to the main item.
* For example, in a local environment, the module copies values from the "local" item into the main “Settings” item.
* This same action is triggered on the Initialize pipeline and Package Installed event.

Additionally, changes can be applied in any other custom situation by code, using ItemPatchingManager.Apply().
