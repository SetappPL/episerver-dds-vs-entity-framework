# Custom Dynamic Data Store table in EpiServer 11 vs Entity Framwork

This is an implementation of a custom big table in Episerver's DDS and its equivalent in Entity Framwork. The solution contains also tests which compare performance of a custom DDS table and Entity Framwork.

To run the solution open it in Visual Studio, go to Package Manager Console and run a command `Update-Package -reinstall`. When a question about replacing existing files appears, input `A` for all. Then click F5 to run it.

To run the tests, go to `/performance-tests` url. It may take about 40 minutes for the tests to run. After that time a page with results will be displayed. 