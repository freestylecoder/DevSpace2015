# DevSpace
This is the website used for the DevSpace Technical Conference in Huntsville, Alabama.

It consists of two main parts. The WebSite project is the actual website. It is a standard ASPX site. There is nothing special about the ASPX, and I don't REALLY plan on using the features. I merely wanted a master page. The publish profiles were removed from the site.

The Api is the server side. It is a MVC Web Api v2 project. Again, the project that publishes to Azure (in this case, a worker role) has been removed. When you create your Deployment project, make sure you add configuration settings for "DatabaseConnectionString" and "AllowedSites".
