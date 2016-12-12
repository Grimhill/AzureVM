using Microsoft.Azure;
//using Microsoft.Rest.Azure;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Management.Compute;
using Microsoft.WindowsAzure.Management.Compute.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using QueueLogic;

namespace CreateVM
{
    public class VMManager
    {
       
        //download publish setting from your azure subscritpion, take id and certificate
        public const string base64EncodedCertificate = "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";
        public const string subscriptionId = "d5558e68-747a-40f8-94a7-59575fа1ef29";        

        public static Microsoft.Azure.CertificateCloudCredentials cloudCredentials = 
            new Microsoft.Azure.CertificateCloudCredentials(subscriptionId, new System.Security.Cryptography.X509Certificates.X509Certificate2(Convert.FromBase64String(base64EncodedCertificate)));
        public static string relatedStorageAccountName = "clean"; //enter storage name, in wich you wont store VM`s VHD (if usinf VM images)
                

        #region VM public IP
        /// <summary>
        ///узнаем внешний общедоступный IP
        /// </summary>
        public string VMPublicIP(Data que)
        {
            ComputeManagementClient client = new ComputeManagementClient(cloudCredentials);
            var deployment = GetAzureDeyployment(que.CloudServiceName, DeploymentSlot.Production);

            return deployment.VirtualIPAddresses.FirstOrDefault().Address;
        }
        #endregion

        #region VM status
        /// <summary>
        ///мониторим статус машины
        /// </summary>
        public bool VMStatus(Data que)
        {
            ComputeManagementClient client = new ComputeManagementClient(cloudCredentials);
            var deployment = GetAzureDeyployment(que.CloudServiceName, DeploymentSlot.Production);

            //return deployment.RoleInstances.Any(x => x.InstanceName == que.VirtualMachineName && x.InstanceStatus == "ReadyRole"); //в случае если будем в одной облачной службе создавать машины
            return deployment.RoleInstances.Any(x => x.InstanceStatus == "ReadyRole");
            //foreach (var instance in deployment.RoleInstances)
            //{
            //    Console.WriteLine("Name: {0}, State: {1}, PowerState: {2}", instance.InstanceName, instance.InstanceStatus, instance.PowerState);
            //}
        }
        #endregion        

        #region Create VM
        public void QuickCreateVM(Data que)
        {
            try
            {
                ComputeManagementClient client = new ComputeManagementClient(cloudCredentials);
                string CloudServ = que.CloudServiceName;  //CloudService name
                string VMname = que.VirtualMachineName; //VM name

                string parameters = string.Empty;
                parameters = que.GetParams();

                try //check if deployment exist in current cloud service
                {
                    var cho = client.Deployments.GetByName(CloudServ, CloudServ);
                                
                    //create one more vm in cloud service, if deployment already exist
                    VirtualMachineCreateParameters createVM = new VirtualMachineCreateParameters
                    {
                        RoleName = VMname,
                        RoleSize = que.VMSize,
                        ProvisionGuestAgent = true,
                        VMImageName = que.VirtualMachineSelected,
                        ResourceExtensionReferences = new List<ResourceExtensionReference>()
                    {
                        new ResourceExtensionReference
                        {
                            ReferenceName = "CustomScriptExtension",
                            Name = "CustomScriptExtension",
                            Publisher = "Microsoft.Compute",
                            Version = "*",
                            ResourceExtensionParameterValues = new List<ResourceExtensionParameterValue>()
                            {
                                new ResourceExtensionParameterValue
                                {
                                    //testing variants for custom script
                                    Key = "CustomScriptExtensionPublicConfigParameter",
                                    //Value = "{\"fileUris\": [\"" + "https://teststorageaccount.blob.core.windows.net/testcontainer" + "\"], \"commandToExecute\":\"powershell -ExecutionPolicy Unrestricted -file " + "ScriptName.ps1" + "\"}",
                                    //Value = "{\"fileUris\": [\"" + "https://studvm.blob.core.windows.net/filestorage/firstscript.ps1" + "\"], \"commandToExecute\":\"powershell -file " + "firstscript.ps1 -FirstSoft $false -SecondSoft $true -ThirdSoft $true" + "\"}",
                                    Value = "{\"fileUris\": [\"" + "https://studvm.blob.core.windows.net/filestorage/firstscript.ps1" + "\"], \"commandToExecute\":\"powershell -ExecutionPolicy Unrestricted -file " + "firstscript.ps1 " + parameters + "\"}",
                                    //Value = "{\"fileUris\": [\"" + "https://studvm.blob.core.windows.net/filestorage/mkdir.ps1" + "\"], \"commandToExecute\":\"powershell -ExecutionPolicy Unrestricted -file " + "mkdir.ps1 c:\\hello_from_customscriptextension" + "\"}",
                                    Type = "Public"
                                },

                                new ResourceExtensionParameterValue
                                {
                                    //storage where script located
                                    Key = "CustomScriptExtensionPrivateConfigParameter",
                                    Value = "{\"storageAccountName\":\"" + "storagename" + "\",\"storageAccountKey\": \"" + "storagekey" + "\"}",                                    
                                    Type = "Private"
                                }
                            }
                        }
                    },

                    ConfigurationSets = new List<ConfigurationSet>()
                    {
                        new ConfigurationSet
                        {
                            ConfigurationSetType = ConfigurationSetTypes.WindowsProvisioningConfiguration,
                            EnableAutomaticUpdates = false,
                            ResetPasswordOnFirstLogon = false,
                            ComputerName = VMname,
                            AdminUserName = que.AdminUser,
                            AdminPassword = que.Password,
                            TimeZone = "Eastern European Time",
                            // properties that define a domain to which the Virtual Machine will be joined
                            DomainJoin = new DomainJoinSettings
                            {
                                DomainToJoin = "ActiveD.com",
                                Credentials = new DomainJoinCredentials
                                {
                                    //admin`s user and password of machine on wich domain controller
                                    Domain = "ActiveD.com", //domain name
                                    UserName = "anytime",
                                    Password = "Q1w2e3r4"
                                }
                            }
                        },

                        new ConfigurationSet
                        {
                            ConfigurationSetType = ConfigurationSetTypes.NetworkConfiguration,
                            SubnetNames = new List<string> { "subnet" },                       //the name of the existin Net to which domain controller belong
                            InputEndpoints = new BindingList<InputEndpoint>                     //add more endpoints if need
                            {
                                new InputEndpoint { Name = "RDP", Protocol = "tcp", LocalPort = 3389 },                                
                                new InputEndpoint { Name = "PowerShell", Protocol = "tcp", LocalPort = 5986}
                            }
                        }
                    },
                    };
                    //create new VM in exist deployment
                    client.VirtualMachines.Create(CloudServ, CloudServ, createVM);
                }
                
                catch //if deployment is not exist create it
                {
                    //create deployment in cloud service, and first vm in it

                    //STEP1:Create Hosted Service
                    //Azure VM must be hosted in a hosted cloud service.

                    //createCloudService(vmName, que.RegionSelected, null);  //comment if cloud serivce already exist

                    //STEP2:Construct VM Role instance
                    var vmRole = new Role()
                    {
                        DefaultWinRmCertificateThumbprint = CertificateThumbprintAlgorithms.Sha1,
                        RoleType = VirtualMachineRoleType.PersistentVMRole.ToString(),
                        RoleName = VMname,
                        Label = CloudServ,
                        RoleSize = que.VMSize, //VirtualMachineRoleSize.Small,  //que.Size
                        ConfigurationSets = new List<ConfigurationSet>(),
                        VMImageName = que.VirtualMachineSelected,
                        ProvisionGuestAgent = true,
                        //if create VM form gallery images
                        //OSVirtualHardDisk = new OSVirtualHardDisk() 
                        //{
                        //    MediaLink = getVhdUri(string.Format("{0}.blob.core.windows.net/vhds", relatedStorageAccountName)),
                        //    SourceImageName = GetSourceImageNameByFamliyName(que.VirtualMachineSelected)
                        //}
                    };

                    //For custom script usage
                    ResourceExtensionReference ResExtRef = new ResourceExtensionReference
                    {
                        ReferenceName = "CustomScriptExtension",
                        Name = "CustomScriptExtension",
                        Publisher = "Microsoft.Compute",
                        Version = "*",
                        ResourceExtensionParameterValues = new List<ResourceExtensionParameterValue>()
                    {
                        new ResourceExtensionParameterValue
                        {
                        Key = "CustomScriptExtensionPublicConfigParameter",                        
                        Value = "{\"fileUris\": [\"" + "https://studvm.blob.core.windows.net/filestorage/firstscript.ps1" + "\"], \"commandToExecute\":\"powershell -ExecutionPolicy Unrestricted -file " + "firstscript.ps1 " + parameters + "\"}",                        
                        Type = "Public"
                        },

                        new ResourceExtensionParameterValue
                        {
                        Key = "CustomScriptExtensionPrivateConfigParameter",
                        Value = "{\"storageAccountName\":\"" + "storagename" + "\",\"storageAccountKey\": \"" + "storagekey" + "\"}",                       
                        Type = "Private"
                        }
                    }
                    };

                    //Для линукса другой конфиг сет тайп
                    ConfigurationSet configSet = new ConfigurationSet
                    {
                        ConfigurationSetType = ConfigurationSetTypes.WindowsProvisioningConfiguration,
                        EnableAutomaticUpdates = false,
                        ResetPasswordOnFirstLogon = false,
                        ComputerName = VMname,
                        AdminUserName = que.AdminUser,
                        AdminPassword = que.Password,
                        TimeZone = "Eastern European Time",
                        // properties that define a domain to which the Virtual Machine will be joined
                        DomainJoin = new DomainJoinSettings
                        {
                            DomainToJoin = "ActiveD.com",
                            Credentials = new DomainJoinCredentials
                            {
                                //admin`s user and password of machine on wich domain controller
                                Domain = "ActiveD.com",  //domain name
                                UserName = "anytime",
                                Password = "Q1w2e3r4"
                            }
                        }
                    };

                    //endpoints
                    ConfigurationSet NetworkconfigSet = new ConfigurationSet
                    {
                        ConfigurationSetType = ConfigurationSetTypes.NetworkConfiguration,
                        SubnetNames = new List<string> { "subnet" },                       //the name of the existin Net to which domain controller belong
                        InputEndpoints = new BindingList<InputEndpoint>
                        {
                            new InputEndpoint { Name = "RDP", Protocol = "tcp", Port = 3389, LocalPort = 3389 },                    
                            new InputEndpoint { Name = "PowerShell", Protocol = "tcp", Port = 5986, LocalPort = 5986}
                            }
                    };

                    //adding roles and configs
                    vmRole.ConfigurationSets.Add(configSet);
                    vmRole.ConfigurationSets.Add(NetworkconfigSet);
                    vmRole.ResourceExtensionReferences.Add(ResExtRef);
                    //vmRole.ResourceExtensionReferences = null;

                    //STEP3: Add Role instance to Deployment Parmeters
                    List<Role> roleList = new List<Role>() { vmRole };
                    VirtualMachineCreateDeploymentParameters createDeploymentParams = new VirtualMachineCreateDeploymentParameters
                    {
                        Name = CloudServ,
                        Label = CloudServ,
                        Roles = roleList,
                        DeploymentSlot = DeploymentSlot.Production,
                        VirtualNetworkName = "Actived", //Specifies the name of an existing virtual network to which the deployment will belong
                    };

                    //STEP4: Create a Deployment with VM Roles.
                    client.VirtualMachines.CreateDeployment(CloudServ, createDeploymentParams);
                }
                //Console.WriteLine("Create VM success");
            }
            catch (CloudException e)
            {

                throw e;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //return adress and name for VHD in storage
        private static Uri getVhdUri(string blobcontainerAddress)
        {
            var now = DateTime.UtcNow;
            string dateString = now.Year + "-" + now.Month + "-" + now.Day;// +now.Hour + now.Minute + now.Second + now.Millisecond; //create name as date year + month + day

            var address = string.Format("http://{0}/{1}-650.vhd", blobcontainerAddress, dateString);
            return new Uri(address);
        }

        private static void createCloudService(string cloudServiceName, string location, string affinityGroupName = null)
        {
            ComputeManagementClient client = new ComputeManagementClient(cloudCredentials);

            HostedServiceCreateParameters hostedServiceCreateParams = new HostedServiceCreateParameters();
            if (location != null)
            {
                hostedServiceCreateParams = new HostedServiceCreateParameters
                {
                    ServiceName = cloudServiceName,
                    Location = location,
                    Label = EncodeToBase64(cloudServiceName),
                };
            }
            else if (affinityGroupName != null)
            {
                hostedServiceCreateParams = new HostedServiceCreateParameters
                {
                    ServiceName = cloudServiceName,
                    AffinityGroup = affinityGroupName,
                    Label = EncodeToBase64(cloudServiceName),
                };
            }
            try
            {
                client.HostedServices.Create(hostedServiceCreateParams);
            }
            catch (CloudException e)
            {
                throw e;
            }

        }

        private static string EncodeToBase64(string toEncode)
        {
            byte[] toEncodeAsBytes
            = System.Text.ASCIIEncoding.ASCII.GetBytes(toEncode);
            string returnValue
                  = System.Convert.ToBase64String(toEncodeAsBytes);
            return returnValue;
        }

        private static string GetSourceImageNameByFamliyName(string imageFamliyName)
        {
            ComputeManagementClient client = new ComputeManagementClient(cloudCredentials);
            var results = client.VirtualMachineVMImages.List();
            var disk = results.Where(o => o.ImageFamily == imageFamliyName).FirstOrDefault();

            if (disk != null)
            {
                return disk.Name;
            }
            else
            {
                throw new CloudException(string.Format("Can't find {0} OS image in current subscription"));
            }
        }
        #endregion

        #region List Images
        /// <summary>
        /// Retrieves a list of the OS images from the image repository
        /// </summary>
        public void ListImages()
        {
            ComputeManagementClient client = new ComputeManagementClient(cloudCredentials);
            var results = client.VirtualMachineVMImages.List();
            foreach (var image in results)
            {
                Console.WriteLine(string.Format("Name:{0}", image.Name));
            }
        }
        #endregion

        #region List VM
        public void ListVM()
        {
            ComputeManagementClient client = new ComputeManagementClient(cloudCredentials);
            var hostedServices = client.HostedServices.List();
            foreach (var service in hostedServices)
            {
                var deployment = GetAzureDeyployment(service.ServiceName, DeploymentSlot.Production);
                if (deployment != null)
                {
                    if (deployment.Roles.Count > 0)
                    {
                        foreach (var role in deployment.Roles)
                        {
                            if (role.RoleType == VirtualMachineRoleType.PersistentVMRole.ToString())
                            {
                                Console.WriteLine(role.RoleName);
                            }

                        }
                    }
                }
            }
        }

        private static DeploymentGetResponse GetAzureDeyployment(string serviceName, DeploymentSlot slot)
        {
            ComputeManagementClient client = new ComputeManagementClient(cloudCredentials);
            try
            {
                return client.Deployments.GetBySlot(serviceName, slot);

            }
            catch (CloudException ex)
            {

                if (ex.ErrorCode == "ResourceNotFound")
                {
                    return null;
                }
                else
                {
                    throw ex;
                }
            }
        }
        #endregion

        #region Restart VM
        public void RestartVM()
        {
            string vmName = "mvademovm";
            string deploymentName = "forvmexpapp";
            string hostserviceName = "forvmexpapp";

            ComputeManagementClient client = new ComputeManagementClient(cloudCredentials);
            client.VirtualMachines.Restart(hostserviceName, deploymentName, vmName);
        }

        #endregion

        #region Delete VM
        public void DeleteVM()
        {
            string vmName = "yuan2013vm";
            string deploymentName = "yuan2013vm";
            string hostserviceName = "yuan2013vm";

            ComputeManagementClient client = new ComputeManagementClient(cloudCredentials);
            client.VirtualMachines.Delete(hostserviceName, deploymentName, vmName, true);
        }
        #endregion

    }
}

