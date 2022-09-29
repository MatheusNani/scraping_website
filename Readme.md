This project simulates an web scraping a webpage.
It gets some especific information from a website.


#To run this application go under the directory 'ConsoleApp1' using your CLI, then execute the command line 'dotnet run'


#1)
There is a problem with de Min and Max calculation, just ordering by the number does not work once we have: 
66mm, 10cm, 1m

Here the max value is 1m.

I would add a new property called "Rank" to my dataStorage so when I needed I could just:
OrderBy(o => o.Rank).ThenBy(o => o.WaveHeight) getting the highest rank and highest wave. Considering mm = 1, cm = 2 and m = 3

#2) 
I would improve handling with errors at the MakeRequest() once the website can go offline. 
 - Checking the website messages when error occurs.
 
#3)
 Improve Regex, in some cases I think there is a better approach to get the needed information. 
 - Creating more methods with especifics regex to make it more reusable. 
