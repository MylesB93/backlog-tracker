# Deployment Configuration

This document describes the automated deployment setup for the BacklogTracker application.

## Overview

The application uses GitHub Actions to automatically deploy to production when changes are pushed to the `master` branch. The deployment uses Web Deploy (MSDeploy) to publish the application to the hosting environment.

## GitHub Actions Workflow

The deployment workflow (`.github/workflows/deploy-production.yml`) performs the following steps:

1. **Checkout code** - Retrieves the latest code from the repository
2. **Setup .NET 9** - Configures the .NET 9 SDK
3. **Restore dependencies** - Restores NuGet packages
4. **Build solution** - Compiles the application in Release configuration
5. **Run tests** - Executes all unit tests
6. **Publish application** - Creates deployment package
7. **Deploy via Web Deploy** - Publishes to production server

## Required GitHub Secrets

To enable automated deployment, you must configure the following secrets in your GitHub repository:

### Setting Up Secrets

1. Navigate to your GitHub repository
2. Go to **Settings** ? **Secrets and variables** ? **Actions**
3. Click **New repository secret** for each of the following:

### Secret Configuration

| Secret Name | Description | Example/Source |
|-------------|-------------|----------------|
| `MSDEPLOY_SERVICE_URL` | The Web Deploy service URL | `https://win6036.site4now.net:8172/msdeploy.axd?site=mylesb93-001-site1` |
| `MSDEPLOY_SITE_NAME` | The IIS application path/site name | `mylesb93-001-site1` |
| `MSDEPLOY_USERNAME` | Web Deploy username | `mylesb93-001` |
| `MSDEPLOY_PASSWORD` | Web Deploy password | *(Your hosting provider password)* |

### Where to Find These Values

All values can be found in your Web Deploy publish profile:
- **File location**: `BacklogTracker.Web/Properties/PublishProfiles/mylesb93-001-site1 - Web Deploy.pubxml`

**Mapping:**
- `MSDEPLOY_SERVICE_URL` ? `<MSDeployServiceURL>` element
- `MSDEPLOY_SITE_NAME` ? `<DeployIisAppPath>` element
- `MSDEPLOY_USERNAME` ? `<UserName>` element
- `MSDEPLOY_PASSWORD` ? Obtain from your hosting provider (SmarterASP.NET control panel)

## Security Notes

?? **Important Security Considerations:**

- Never commit passwords or sensitive credentials to the repository
- Always use GitHub Secrets for sensitive values
- The `_SavePWD` is set to `false` in the publish profile to prevent password storage
- GitHub Secrets are encrypted and only exposed to workflow runs

## Deployment Trigger

The workflow automatically triggers on:
- Push to the `master` branch

To deploy manually:
1. Go to **Actions** tab in GitHub
2. Select the "Deploy to Production" workflow
3. Click **Run workflow**

## Monitoring Deployments

To view deployment status:
1. Navigate to the **Actions** tab in your GitHub repository
2. Select the latest workflow run
3. Review the logs for each step

Successful deployments will show a green checkmark and display the site URL in the summary.

## Troubleshooting

### Common Issues

**Build Failures:**
- Check that all NuGet packages are restored correctly
- Verify .NET 9 SDK compatibility with all dependencies

**Test Failures:**
- Review test logs in the workflow output
- Deployment will not proceed if tests fail

**Deployment Failures:**
- Verify all GitHub Secrets are configured correctly
- Check Web Deploy credentials with your hosting provider
- Ensure the hosting environment allows Web Deploy connections
- Review firewall/network settings if connection times out

### Web Deploy Connection Test

To test Web Deploy connectivity locally:
```powershell
msdeploy.exe -verb:dump -source:contentPath="C:\temp" -dest:auto,computerName="https://win6036.site4now.net:8172/msdeploy.axd?site=mylesb93-001-site1",userName="mylesb93-001",password="YOUR_PASSWORD",authType="Basic"
```

## Rollback Procedure

If deployment issues occur:
1. The Web Deploy configuration includes `EnableMSDeployBackup=true`
2. Contact your hosting provider to restore from the automatic backup
3. Or revert the commit in GitHub and push to trigger a new deployment

## Production Site

**URL:** http://mylesb93-001-site1.jtempurl.com/

After deployment, verify the application is running correctly at the production URL.
