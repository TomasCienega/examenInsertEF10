using examenInsertEF10.Context;
using examenInsertEF10.Models;
using examenInsertEF10.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace examenInsertEF10.Controllers
{
    public class EmpleadoController : Controller
    {
        private readonly examenInsertEF10Context _context;
        public EmpleadoController(examenInsertEF10Context context)
        {
            _context=context;
        }
        public async Task<IActionResult> Index(int idDep)
        {
            var vm = new EmpleadoVM();
            ViewBag.IdSeleccionado = idDep;
            try
            {
                vm.ListaDepartamentos = await _context.Departamentos.ToListAsync();
                if (idDep>0)
                {
                    vm.ListaEmpleados = await _context.Empleados.FromSqlRaw("exec sp_ListarEmpleadosPorIdDep {0}",idDep).ToListAsync();
                }
                else
                {
                    vm.ListaEmpleados = await _context.Empleados.Include(tD => tD.IdDepartamentoNavigation).ToListAsync();
                }
                return View(vm);

            }catch (Exception ex)
            {
                vm.ListaEmpleados = new List<Empleado>();
                vm.ListaDepartamentos = new List<Departamento>();
                Console.WriteLine(ex.Message);
                return View(vm);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Guardar(EmpleadoVM vm)
        {
            try
            {
                await _context.Empleados.AddAsync(vm.EmpleadoModelReference);
                await _context.SaveChangesAsync();
            }catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Borrar(int idEmp)
        {
            // 1. Buscamos el registro real en la BD usando el ID
            var empleado = await _context.Empleados.FindAsync(idEmp);

            if (empleado != null)
            {
                // 2. Si lo encuentra, lo removemos
                _context.Empleados.Remove(empleado);

                // 3. Guardamos los cambios
                await _context.SaveChangesAsync();
                TempData["Mensaje"] = "Empleado eliminado";
            }
            return RedirectToAction("Index");
        }
    }
}
