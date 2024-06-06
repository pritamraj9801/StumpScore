
-- Creating Database
create database StumpScoreDev;

use StumpScoreDev;

-- Creating Country Table
create table Country(
Id int identity(1,1) Primary key,
CountryName varchar(50) not null,
CountryFlag varchar(250)
);

INSERT INTO Country (CountryName,CountryFlag) 
VALUES 
('England','\Content\Images\CountriesFlag\englang.png'),
('India','\Content\Images\CountriesFlag\india.png');

select * from Country;

-- Creating Players Type table
create table PlayerType(
Id int identity(1,1) primary key,
TypeName varchar(20),
PlayerTypeLogo varchar(250)
);

insert into PlayerType(TypeName,PlayerTypeLogo)
values('Batsman','\Content\Images\PlayersType\batter.png'),('Bowler','\Content\Images\PlayersType\bowler.svg'),('AllRounder','\Content\Images\PlayersType\allrounder.svg');

select * from PlayerType;

-- creating players table
create table Player(
Id int identity(1,1) primary key,
JerseyNumber int,
Name varchar(100),
DateOfBirth DateTime,
Age int,
BirthPlace varchar(200),
CountryId int,
PlayerTypeId int,
IsCaptain bit,
MatchesPlayed int,
RunsScored int,
WicketTaken int,
BattingAverage float,
BowlingAverage float,
Centuries int,
HalfCenturies int,
DebutDate datetime,
ICCRanking int,
Picture varchar(200),
foreign key(CountryId) references Country(Id),
foreign key(PlayerTypeId) references PlayerType(Id),
);


select * from Player 
left join Country on Player.CountryId=Country.Id
left join PlayerType on Player.PlayerTypeId=PlayerType.Id;

-- creating match format
create table MatchFormat(
Id int identity(1,1) primary key,
Name varchar(10)
);
insert into MatchFormat(Name) values('T20'),('ODI'),('Test');
select * from MatchFormat;

-- creating tournament table
create table Tournament(
Id int identity(1,1) primary key,
Name varchar(30),
ShortName varchar(10),
StartingDate datetime,
EndingDate datetime,
MatchFormatId int,
foreign key(MatchFormatId) references MatchFormat(Id),
TournamentIcon varchar(250)
);

select * from Tournament

-- creating team
create table Team(
Id int identity(1,1) primary key,
Name varchar(100),
Shortname varchar(20),
TeamOwner varchar(50),
TeamIcon varchar(250),
CaptainId int,
ViceCaptainId int,
WicketKipperId int,
TournamentId int
);

drop table Team
select * from Team

create table PlayerMatch(
Id int identity(1,1) primary key,
PlayerId int,
TeamId int,
TournamentId int,
foreign key(PlayerId ) references Player(Id),
foreign key(TeamId) references Team(Id),
foreign key(TournamentId) references Tournament(Id)
);

select * from PlayerMatch 
left join Player on PlayerMatch.PlayerId = Player.Id
left join Team on PlayerMatch.TeamId=Team.Id
left join Tournament on PlayerMatch.TournamentId=Tournament.Id

create table stadium(
Id int identity(1,1) primary key,
StadiumName varchar(100)
);
INSERT INTO stadium (StadiumName) VALUES ('Wembley Stadium');
INSERT INTO stadium (StadiumName) VALUES ('Old Trafford');
INSERT INTO stadium (StadiumName) VALUES ('Camp Nou');
INSERT INTO stadium (StadiumName) VALUES ('Santiago Bernabéu');
INSERT INTO stadium (StadiumName) VALUES ('Signal Iduna Park');
INSERT INTO stadium (StadiumName) VALUES ('Allianz Arena');
INSERT INTO stadium (StadiumName) VALUES ('San Siro');
INSERT INTO stadium (StadiumName) VALUES ('Etihad Stadium');
INSERT INTO stadium (StadiumName) VALUES ('Anfield');
INSERT INTO stadium (StadiumName) VALUES ('Emirates Stadium');

drop table stadium

select * from stadium

select* from PlayerType

-- Creating Matches Table
create table Matches(
Id int identity(1,1) primary key,
Team1Id int,
Team2Id int,
StadiumId int,
MatchStart datetime,
MatchEnd datetime,
TournamentId int,
foreign key(Team1Id) references Team(Id),
foreign key(Team2Id) references Team(Id),
foreign key(StadiumId) references stadium(Id) ,
foreign key(TournamentId) references Tournament(Id)
);

select
Matches.Id as 'MatchId',
Matches.MatchStart,
Matches.MatchEnd,
stadium.StadiumName,
team1.Id as 'Team1Id',
team2.Id as 'Team2Id',
team1.Name as 'Team1Name',
team2.Name as 'Team2Name',
Matches.TournamentId
from Matches
left join Team as team1 on Matches.Team1Id=team1.Id
left join Team as team2 on Matches.Team2Id=team2.Id
left join stadium on Matches.StadiumId=stadium.Id
where Matches.TournamentId = 2

select * from Matches where Id = 1 and TournamentId=2

select * from team where Id = 1;

select
Player.Id as 'PlayerId',
Player.JerseyNumber,
Player.Name,
Player.DateOfBirth,
Player.Age,
Player.BirthPlace,
Player.CountryId,
Player.PlayerTypeId,
Player.IsCaptain,
Player.MatchesPlayed,
Player.RunsScored,
Player.WicketTaken,
Player.BattingAverage,
Player.BowlingAverage,
Player.Centuries,
Player.HalfCenturies,
Player.DebutDate,
Player.ICCRanking,
Player.Picture
from Playermatch
left join Player on Playermatch.PlayerId = Player.id
where teamId = 2 and TournamentId=2;

select * from PlayerMatch

select * from Stadium where Id = 2


--------------- clearing the tables
select * from Player
delete from 
Player where Id > 0

select * from PlayerMatch
delete from PlayerMatch where Id > 0

select * from Tournament
delete from Tournament where Id > 0

select * from Matches
delete from Matches where Id> 0

select * from Team
delete from Team where Id > 0



select
Matches.Id as 'MatchId',
Matches.MatchStart,
Matches.MatchEnd,
stadium.StadiumName,
team1.Id as 'Team1Id',
team2.Id as 'Team2Id',
team1.Name as 'Team1Name',
team2.Name as 'Team2Name',
Matches.TournamentId
from Matches
left join Team as team1 on Matches.Team1Id=team1.Id
left join Team as team2 on Matches.Team2Id=team2.Id
left join stadium on Matches.StadiumId=stadium.Id
where Matches.TournamentId =3


select * from player where Id = 1017


update Player 
set CountryId = 1
where Id = 1017