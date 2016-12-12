This project is a windows service, which checking Azure Queue.
If there any data service starting to create Vm v1.
According to creating options, vm will be joined into domain and existing Net. Domain controller based on existing machine.
Also domain controller VM joined into Net, created on portal. New machine will be joined in the same net.
By using option "custom script" in this project downloading several soft installers and beginning to install them
after vm deployment.

The idea was that the service will run on the machine - domain controller, and include new machines to the domain and network.

WebintVM project can be used to send VM`s deployment data into Azure Queue.
This data saving into data base in the cloud.

For VM creating you need to download publish setting from your azure subscription to take id and certificate.
Don`t forget to change app\web.config files to change connection string and storage credentials.

Also project in debug mod.