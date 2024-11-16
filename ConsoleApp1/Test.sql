USE Prova_Codice_Fiscale;


CREATE TABLE Persona
(
	id int primary key identity (1,1),
	nome varchar(100),
	cognome varchar(100)
);

insert into Persona (nome,cognome) Values ('giovanni', 'arnosti')

select * from Persona 

drop table Persona

DROP Database Prova_Codice_Fiscale;

use master;