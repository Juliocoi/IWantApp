namespace IWantApp.Endpoints.Employees;
// usando o record ao invés de uma classe, pois o Record se torna uma classe.
//Ler sobre record
public record EmployeeRequest(string Email, string Password, string Name, string EmployeeCode);
