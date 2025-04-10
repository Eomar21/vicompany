# Developer Log

## Objective 1 :
- Read the documentation carefully understood what needs to be done from there.
- Looked into the unit test as I assumed this where all the criteria will be located assuming TDD is being applied here which was the case "Thanks for that :) "
- Implemented all the criteria needed from the unit test which was straight forward and saw the unit test passing as I went through each criteria.
## Objective 2 :
- Followed the approach to have the client factory to create my clients and take care of that creating httpclinet instances for better resource management.
- I used newtonsoft converter and created equivalent object to the TickResponse to make things cleaner.
- Decided to keep DataTime in the Quote object as is in UTC format and make the time in UI follow iso8601.instead.
- Added few comments to refactor DummyQuoteRepository to be Async later in Objective 3 

## Objective 3 :
- I decided to change the IQuotesRepository although solid says extend and never change interfaces but I made an exception here as I know this is not being used anywhere and also that I prefer to have everything by default Async.