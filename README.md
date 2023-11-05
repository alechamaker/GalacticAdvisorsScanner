# Scanner

The purpose of this project is to create a code sample for an application process for [GalacticAdvisors](https://www.galacticadvisors.com/).
<hr/>

## Timesheet Breakdown

### First 30 minutes (0:30)
> The first 30 minutes consisted of me debating with myself on which language/framework to work with. I decided to create a MVP for the user story. I wanted to start simple, create something I could work off of and add to later. I knew that I wouldn't be able to address all the user's concerns in a 2 hour window. I decided first to go with Python, then after re-reading the email -- I decided to go with C#.

> I jumped into coding, I knew I wanted to start with the easiest of the file formats (txt). So I started researching how I could effectly scan drives.

> I was able to complete the drive and directory traversal at this point.

### First Hour (1:00)
> I wanted to make sure that I was effectly utilizing resources, so the majority of this 30 minute block went to writing code to parallelize some of the processes.

> I found some promising regex expressions on the web and implemented them into the process at this point.

> I spent some time troubleshooting some problems with the Tasks I had setup.

> At this point I was able to write the skeleton of the DriveScanner and FileScanner classes.

### 1:30

> At this point I added capability for more filetypes, as I was only targeting txt files.

> I had to refactor some of my code in order to account for multiple file types

> I also realized that my regex was only targeting patterns that appeared at the beginning of the line. I adjusted this. Then I came to find many false positives. I adjusted my regex accordingly to find a happy medium.

### Second Hour (2:00)
> The final 30 minutes were spent mainly cleaning up some the messes I had made along the way. Although, I know the time limit was not intended to create stress, I knew my time was ticking.

> I refactored the output of my tasks to return the match, the file, and the index where the match was found. Although I was not able to get to a 'report' level. I figured this was better than just matches.

> I toyed with some of the file extension patterns. I found that using a `*.*` pattern resulted is massive RAM usage, so I tried to adjust my code to account for this, but I came up short so I went back to the small list of plain text formats I was using before.
