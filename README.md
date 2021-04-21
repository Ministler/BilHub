# BilHub

BilHub is a comprehensive classroom helper for instructors, teaching assistans and students especially designed for classes that include teamwork. <br />
<br />
- There will be 3 user types; Instructor, TA, and Student.
- Instructor users can host lectures, eg: CS 319 SPRING 2021. 
- An instructor can assign TA and Student users via a specific code or link.
- Students can form project groups until the deadline. 
- Students that want to be in the same group can form pseudogroups. 
- Students can review other students' pages to get information. 
- Student pages will include students' info and other projects that students conducted, eg: CS 102 SPRING 2019 project that students enrolled in. 
- When the project group formation deadline comes, the system will assign groups automatically by satisfying the most possible number of pseudogroups. 
- Each group will have a page that includes project name and student names. 
- For each group page, TA's can assign assignments to group pages. eg: Report 1 assignment. 
- TA can also assign peer grading assignments that students can grade each other for assignments. 
- The visibility of those assignments and peer gradings will be decided by TA. Other students can only view those assignments if TA enables them. 
- Assignments can be done by uploading files or sharing a GitHub link.
- Users can search for other users' pages to review students' other projects. 
- Users can also search for previous semesters' project groups to get information. The visibility of those project pages will be decided by the previous TA users.  
- Each user will have their pages. Their page will include information, projects, and hosted project sections. Any empty section will not be displayed. 

# Contributors
Mustafa Çağrı Durgut ([mcagridurgut](http://github.com/mcagridurgut)) <br />
Halil Özgür Demir ([hozgurde](http://github.com/hozgurde)) <br />
Aybala Karakaya ([aaybala](http://github.com/aaybala)) <br />
Yusuf Miraç Uyar ([Y-Yosu](http://github.com/Y-Yosu)) <br />
Barış Ogün Yörük ([barisoyoruk](http://github.com/barisoyoruk)) <br />
Oğuzhan Özçelik ([ozc0](http://github.com/ozc0)) <br />

Database'i kurmak icin:<br />
dotnet ef --startup-project ./MyMusic.Api/MyMusic.Api.csproj database update<br />
BilHub.Api/appsettings.Development.json -> "ConnectionStrings": {
    "Default": "server=.\\SQLEXPRESS; database=BilHubTest; user id=USERNAME; password=PASSWORD"
  }<br />
Calistirmak icin<br />
dotnet run -p BilHub.Api/BilHub.Api.csproj


