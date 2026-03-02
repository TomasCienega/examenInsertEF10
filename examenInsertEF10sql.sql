--DESKTOP-JJ9DM3F\SQLEXPRESS

create database examenInsertEF10
use examenInsertEF10
create table Departamento 
(
	idDepartamento int identity(1,1) not null,
	nombreDepartamento varchar(50) not null,
	constraint PK_Departamento primary key (idDepartamento)
)
insert into Departamento(nombreDepartamento)values('Marketing'),('IT'),('Ventas'),('Compras'),('Redes')
select * from Departamento

create table Empleado 
(
	idEmpleado int identity(1,1) not null,
	nombreEmpleado varchar(100) not null,
	idDepartamento int,
	constraint PK_Empleado primary key (idEmpleado),
	constraint FK_EmpleadoDepartamento foreign key (idDepartamento)
										references Departamento(idDepartamento)
)
insert into Empleado(nombreEmpleado,idDepartamento)values('Adrian',1),('Carlos',3),('Alan', 5)
select * from Empleado

create procedure sp_ListarEmpleadosPorIdDep
(
	@idDepartamento int
)
as
begin 
	select e.idEmpleado,e.nombreEmpleado,d.idDepartamento,d.nombreDepartamento
	from Empleado e
	inner join Departamento d
	on e.idDepartamento = d.idDepartamento
	where d.idDepartamento = @idDepartamento
end