set "modfolder=BOStuffPack"
set "steampath=%programfiles(x86)%\Steam"
set "modinfofolder=ModInfo"

::===================================================================
set "modpath=%steampath%\steamapps\common\Brutal Orchestra\BrutalOrchestra_Data\StreamingAssets\mods\%modfolder%"

::DLL
set "dllname=%assemblyname%.dll"
echo Copying DLL
copy "%outdir%%dllname%" "%modpath%\plugins\%dllname%"

::MODINFO
if exist "%modinfofolder%" (
    if exist "%modinfofolder%\modinfo.config" (
        echo Copying modinfo.config
        copy "%modinfofolder%\modinfo.config" "%modpath%\modinfo.config"
    )
    if exist "%modinfofolder%\thumbnail.png" (
        echo Copying thumbnail
        copy "%modinfofolder%\thumbnail.png" "%modpath%\thumbnail.png"
    )
)