namespace CashInTerminal.Enums
{
    enum UpdateStatuses
    {
        NoUpdateAvailable,
        UpdateAvailable,
        UpdateRequired,
        NotDeployedViaClickOnce,
        DeploymentDownloadException,
        InvalidDeploymentException,
        InvalidOperationException
    }
}
