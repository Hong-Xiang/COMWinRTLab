$WindowsSdkDir = "C:\Program Files (x86)\Windows Kits\10"
$SdkVersion = "10.0.26100.0"
midl /winrt `
/metadata_dir "$WindowsSdkDir\References\$SdkVersion\windows.foundation.foundationcontract\4.0.0.0" `
/h "nul" /nomidl `
/reference "$WindowsSdkDir\References\$SdkVersion\Windows.Foundation.FoundationContract\4.0.0.0\Windows.Foundation.FoundationContract.winmd" `
Shape.idl
# /reference "$WindowsSdkDir\References\$SdkVersion\Windows.Foundation.UniversalApiContract\6.0.0.0\Windows.Foundation.UniversalApiContract.winmd" `
# /reference "$WindowsSdkDir\References\$SdkVersion\Windows.Networking.Connectivity.WwanContract\2.0.0.0\Windows.Networking.Connectivity.WwanContract.winmd" `