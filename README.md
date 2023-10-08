# Aether Compass

> :warning: My apologise:
> This plugin is not being actively worked on at the moment,
> though it is not entirely dead at the moment either (and big thanks to community contributions!).
>
> For those who are looking for the latest version since Dalamud's API9 update,
> there is a testing version available in my custom repo.
> See [below](#installation) for the installation guide.
>
> There is no guarantee the plugin will function as expected as I am unable to test it extensively.
> But any contribution (PRs or issues) is still welcome --- just don't expect prompt responses from me.

---

> <img src="./res/img/icon_by_arkfrostlumas.png" width="96"/>
> Have you ever run into something when looking at your compass ... oooops!

_Aether Compass_ is an FFXIV Dalamud plugin that has a compass (a set of compasses, actually) 
that automatically detects certain objects/NPCs such as Aether Currents nearby and shows where they are.

Detected objects/NPCs are displayed in various ways, 
notably through markers on your screen that point to the locations of these objects/NPCs.
Optionally, it can notify you through Chat and/or Toast messages.

Currently supports detecting:
- Aether Currents
- Mob Hunt Elite Marks (Notorious Monsters)
- Gathering Points
- Eureka Elementals (by apetih)
- *\[Experimental\] Quest-related NPCs/Objects*
- *\[Experimental\] Island Sanctuary Gathering Objects and Animals*

**NOTE:** Because most objects/NPCs are not loaded 
when they are too faraway or when there are too many entities nearby (such as too many player characters), 
they will not be detected in this case.


## Installation

[XIVLauncher](https://github.com/goatcorp/FFXIVQuickLauncher) is required to install and run the plugin.

The plugin is currently only available in my custom repo.
To access it, you can add my [Dalamud plugin repo](https://github.com/yomishino/MyDalamudPlugins) to Dalamud's Custom Plugin Repositories,
and look for the plugin "Aether Compass [Preview]" in Plugin Installer's available plugins.

If you cannot find a release version (i.e. not a testing version) of it,
then the plugin is either only available as a testing plugin, or it is not updated at all.

In the former case, you can find it by enabling the setting "Get plugin testing builds" in Dalamud.

:warning: **Please be cautious in doing so, as testing plugins are expected to be unstable.
There could be major bugs that may even crash your game client in some worse cases.**
In addition, by enabling this setting you might also receive unstable test builds of other plugins.


<!--
There are two sources from which you can install the plugin:
- the official repo (the "standard" version, but as a testing plugin), or
- my custom repo (the "preview" version)

:exclamation: For users who previously installed through my custom repo:
If the version you installed from my custom repo does not have `[Preview]` in its name,
please **uninstall it and reinstall** through one of the methods for the correct version.

That version is obsolete and is no longer able to receive update.

### Through Official Repo

Aether Compass is currently a testing plugin in the Dalamud official repo.

To install the plugin from the official repo, you will need to enable the setting "Get plugin testing builds"
under the "Experimental" tab in Dalamud Settings.

> :warning: However, please be cautious that by enabling this setting you will also be receiving testing builds of other plugins,
> which, as the name suggests, are still in testing and may be unstable.


### Through My Custom Repo

Add my [Dalamud plugin repo](https://github.com/yomishino/MyDalamudPlugins) to Dalamud's Custom Plugin Repositories.

Once added, look for the plugin "Aether Compass [Preview]" in Plugin Installer's available plugins.

You do not need to enable the setting "Get plugin testing builds" in Dalamud in order to install the preview version,
although you could opt to do so and receive unstable test builds occasionally.

> :warning: If you would like to install the Preview version this way 
> but have also installed the plugin [through the official repo as described above](#through-official-repo), 
> please **disable** the standard version from the official repo **before** installing and running 

### Notes on the Two Versions

Normally the two versions are the same, despite that the preview version has "[Preview]" in its name.

The "standard" version installed through the official repo tends to receive an update a bit later 
due to the review process.
But that should not be a real problem most of the time (I guess?).

You can click below for more detailed explanation for why there is such a mess. But **tl;dr**

<details>

Since an update introduced in Dalamud some time around the release of Patch 6.2 (probably),
plugins in custom repos are not allowed to have the same name as plugins in the official repo for security reasons.

Previously, Aether Compass had been on both repos.
Due to the new policy, however, the plugin on my custom repo is no longer available through Dalamud under the same name,
and those who installed that version of the plugin through my custom repo are probably unable to receive any update.

(Those who installed the plugin through the official repo are not likely to be affected by this policy.)

Ideally, everyone should be installing the plugin through the official repo.
But, as it is currently a testing plugin there, this means one must enabling the corresponding setting 
in order to receive testing builds,
and so potentially get exposed to the testing builds of all the other plugins on any repo.
And some of these are truly just, well, testing builds.

So the plugin is renamed as a Preview version so that it can exist alongside the "standard" version in the official repo,
to allow anyone to install it without enabling the "Get plugin testing builds" setting.

Both ways of installation have their own drawbacks.
And the current workaround (by providing a so-called "Preview" version) is very messy on both ends.

That being said, there should presumably be a better way to resolve this issue... But another day, perhaps.
</details>

-->

## Special Thanks

- [apetih](https://github.com/apetih) - For making the Eureka Elementals compass
- [Lumas Arkfrost](https://github.com/ArkfrostLumas) - For the plugin icon
- And thanks to all who have contributed to bug fixes


