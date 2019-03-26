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
AppId={{CEC31B9A-34ED-450F-AF76-5D8827EBFD44}
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
MinVersion=0,6.1.7600

SetupLogging=true
ShowLanguageDialog=yes
ShowTasksTreeLines=true
SignedUninstaller=false
SolidCompression=true
UninstallLogMode=append
UsePreviousLanguage=no
DisableWelcomePage=False
AllowUNCPath=False

[Tasks]

[Files]
Source: "..\bin\Palettes\A.thd"; DestDir: "{app}\Palettes"; Flags: ignoreversion
Source: "{#BINDIR}\Palettes\B.thd"; DestDir: "{app}\Palettes"; Flags: ignoreversion
Source: "{#BINDIR}\Palettes\C.thd"; DestDir: "{app}\Palettes"; Flags: ignoreversion
Source: "{#BINDIR}\Palettes\D.thd"; DestDir: "{app}\Palettes"; Flags: ignoreversion
Source: "{#BINDIR}\Palettes\E.thd"; DestDir: "{app}\Palettes"; Flags: ignoreversion
Source: "{#BINDIR}\Palettes\F.thd"; DestDir: "{app}\Palettes"; Flags: ignoreversion
Source: "{#BINDIR}\Palettes\FOG.thd"; DestDir: "{app}\Palettes"; Flags: ignoreversion
Source: "{#BINDIR}\Palettes\H.thd"; DestDir: "{app}\Palettes"; Flags: ignoreversion
Source: "{#BINDIR}\Palettes\L.thd"; DestDir: "{app}\Palettes"; Flags: ignoreversion
Source: "{#BINDIR}\Palettes\M.thd"; DestDir: "{app}\Palettes"; Flags: ignoreversion
Source: "{#BINDIR}\Palettes\N.thd"; DestDir: "{app}\Palettes"; Flags: ignoreversion
Source: "{#BINDIR}\Palettes\P.thd"; DestDir: "{app}\Palettes"; Flags: ignoreversion
Source: "{#BINDIR}\Palettes\R.thd"; DestDir: "{app}\Palettes"; Flags: ignoreversion
Source: "{#BINDIR}\Palettes\T.thd"; DestDir: "{app}\Palettes"; Flags: ignoreversion
Source: "{#BINDIR}\Palettes\W.thd"; DestDir: "{app}\Palettes"; Flags: ignoreversion
Source: "{#BINDIR}\Palettes\Z.thd"; DestDir: "{app}\Palettes"; Flags: ignoreversion
Source: "{#BINDIR}\ThorusViewer.exe.config"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#BINDIR}\hdf5.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#BINDIR}\hdf5_hl.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#BINDIR}\HTMLparserLibDotNet20.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#BINDIR}\libcurl.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#BINDIR}\MathNet.Numerics.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#BINDIR}\msvcr120.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#BINDIR}\netcdf4.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#BINDIR}\OpenPop.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#BINDIR}\OxyPlot.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#BINDIR}\OxyPlot.Wpf.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#BINDIR}\Thorus.JetPlugin.Default.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#BINDIR}\Thorus.PluginsApi.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#BINDIR}\ThorusCommon.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#BINDIR}\ThorusCommon.Data.NetCdfImporter.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#BINDIR}\ThorusCommon.IO.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#BINDIR}\Xceed.Wpf.Toolkit.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#BINDIR}\zlib1.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#BINDIR}\ThorusSimulation.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#BINDIR}\ThorusViewer.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#BINDIR}\MapContourL2.png"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#BINDIR}\RO4.png"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#BINDIR}\ADJ_LR.thd"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#BINDIR}\albedo.thd"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#BINDIR}\Coastline.thd"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#BINDIR}\ContourRO.thd"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#BINDIR}\elevationMap.thd"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#BINDIR}\GridData.thd"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#BINDIR}\LandWaterMask.thd"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#BINDIR}\SimParams.thd"; DestDir: "{app}"; Flags: ignoreversion

[Icons]
Name: "{group}\Thorus Weather Studio"; Filename: "{app}\ThorusViewer.exe"; WorkingDir: "{app}"; IconFilename: "{app}\ThorusViewer.exe"

[Types]

[Components]

[Run]

[UninstallRun]

[Registry]
