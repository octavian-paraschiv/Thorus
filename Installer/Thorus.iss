;------------------------------
; macrodefinitions
#define BINDIR "..\bin"
#define OUTDIR "..\output"
#define EXTDIR "..\externals"

#define VERSION GetStringFileInfo(BINDIR + "\ThorusCommon.dll", FILE_VERSION)
#define COMPANY GetStringFileInfo(BINDIR + "\ThorusCommon.dll", COMPANY_NAME)
#define PRODUCT GetStringFileInfo(BINDIR + "\ThorusCommon.dll", PRODUCT_NAME)
#define REGENTRY "Software" + "\" + COMPANY + "\" + PRODUCT

[Setup]
;------------------------------
AppID={{9566B126-2205-4E61-8C1C-E6D4D0FC34F0}
;------------------------------
AppVersion={#VERSION}
OutputBaseFilename={#PRODUCT} {#VERSION}
VersionInfoVersion={#VERSION}
VersionInfoTextVersion={#VERSION}
VersionInfoProductVersion={#VERSION}

AppPublisher={#COMPANY}
VersionInfoCompany={#COMPANY}
VersionInfoCopyright={#COMPANY}

AppName={#PRODUCT}
VersionInfoDescription={#PRODUCT}
VersionInfoProductName={#PRODUCT}

DefaultDirName={pf}\{#COMPANY}\{#PRODUCT}
DefaultGroupName={#COMPANY}\{#PRODUCT}

OutputDir={#OUTDIR}

;------------------------------
AllowNoIcons=true
ChangesEnvironment=true
Compression=lzma
DirExistsWarning=yes
DisableFinishedPage=false
DisableReadyPage=false
DisableStartupPrompt=true
EnableDirDoesntExistWarning=true
LanguageDetectionMethod=locale

; WinXP SP3 - miniumum OS required for .NET Framework 4.0
MinVersion=0,5.01.2600sp3

SetupLogging=true
ShowLanguageDialog=yes
ShowTasksTreeLines=true
SignedUninstaller=false
SolidCompression=true
UninstallLogMode=append
UsePreviousLanguage=no
DisableWelcomePage=False

[Tasks]

[Files]
Source: "..\bin\Palettes\A.thd"; DestDir: "{app}\Palettes"; Flags: ignoreversion
Source: "..\bin\Palettes\B.thd"; DestDir: "{app}\Palettes"; Flags: ignoreversion
Source: "..\bin\Palettes\C.thd"; DestDir: "{app}\Palettes"; Flags: ignoreversion
Source: "..\bin\Palettes\D.thd"; DestDir: "{app}\Palettes"; Flags: ignoreversion
Source: "..\bin\Palettes\E.thd"; DestDir: "{app}\Palettes"; Flags: ignoreversion
Source: "..\bin\Palettes\F.thd"; DestDir: "{app}\Palettes"; Flags: ignoreversion
Source: "..\bin\Palettes\FOG.thd"; DestDir: "{app}\Palettes"; Flags: ignoreversion
Source: "..\bin\Palettes\H.thd"; DestDir: "{app}\Palettes"; Flags: ignoreversion
Source: "..\bin\Palettes\L.thd"; DestDir: "{app}\Palettes"; Flags: ignoreversion
Source: "..\bin\Palettes\M.thd"; DestDir: "{app}\Palettes"; Flags: ignoreversion
Source: "..\bin\Palettes\N.thd"; DestDir: "{app}\Palettes"; Flags: ignoreversion
Source: "..\bin\Palettes\P.thd"; DestDir: "{app}\Palettes"; Flags: ignoreversion
Source: "..\bin\Palettes\R.thd"; DestDir: "{app}\Palettes"; Flags: ignoreversion
Source: "..\bin\Palettes\T.thd"; DestDir: "{app}\Palettes"; Flags: ignoreversion
Source: "..\bin\Palettes\W.thd"; DestDir: "{app}\Palettes"; Flags: ignoreversion
Source: "..\bin\Palettes\Z.thd"; DestDir: "{app}\Palettes"; Flags: ignoreversion
Source: "..\bin\ThorusViewer.exe.config"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\bin\hdf5.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\bin\hdf5_hl.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\bin\HTMLparserLibDotNet20.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\bin\libcurl.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\bin\MathNet.Numerics.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\bin\msvcr120.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\bin\netcdf4.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\bin\OpenPop.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\bin\OxyPlot.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\bin\OxyPlot.Wpf.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\bin\Thorus.JetPlugin.Default.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\bin\Thorus.PluginsApi.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\bin\ThorusCommon.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\bin\ThorusCommon.Data.NetCdfImporter.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\bin\ThorusCommon.IO.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\bin\Xceed.Wpf.Toolkit.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\bin\zlib1.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\bin\ThorusSimulation.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\bin\ThorusViewer.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\bin\MapContourL2.png"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\bin\RO4.png"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\bin\ADJ_LR.thd"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\bin\albedo.thd"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\bin\Coastline.thd"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\bin\ContourRO.thd"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\bin\elevationMap.thd"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\bin\GridData.thd"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\bin\LandWaterMask.thd"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\bin\SimParams.thd"; DestDir: "{app}"; Flags: ignoreversion

[Icons]

[Types]

[Components]

[Run]

[UninstallRun]

[Registry]
