using CheckPilot.Server.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;

namespace CheckPilot.Server.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class InvoiceController : ControllerBase
    {
        private readonly SapService _sapService;

        public InvoiceController(SapService sapService)
        {
            _sapService = sapService;
        }

        #region Api para obtener la entrega
        [HttpGet("GetDelivery")]
        public async Task<IActionResult> GetInvoices(
            [FromQuery] string? numAtCard = null,
            [FromQuery] string? docNum = null,
            [FromQuery] string? docEntry = null)
        {
            if (string.IsNullOrEmpty(numAtCard) && string.IsNullOrEmpty(docNum) && string.IsNullOrEmpty(docEntry))
            {
                return BadRequest("Debe ingresar al menos un criterio de búsqueda.");
            }

            string filter = BuildFilter(numAtCard, docNum, docEntry);
            string sessionId = string.Empty;

            try
            {
                sessionId = await _sapService.LoginAsync();

                var rawData = await _sapService.GetInvoicesAsync(filter, sessionId);

                if (string.IsNullOrEmpty(rawData))
                {
                    return NotFound("No se recibieron datos de SAP.");
                }

                var result = JObject.Parse(rawData);
                var invoicesArray = result["value"];

                if (invoicesArray == null || !invoicesArray.Any())
                {
                    return NotFound("No se encontraron facturas con los criterios ingresados.");
                }

                var invoices = invoicesArray.Select(invoice => new
                {
                    DocEntry = (int)invoice["DocEntry"],
                    NumAtCard = (string)invoice["NumAtCard"],
                    DocNum = (int)invoice["DocNum"],
                    DocDate = DateTime.Parse(invoice["DocDate"].ToString()).ToString("yyyy-MM-dd"),
                    DocTime = DateTime.Parse(invoice["DocTime"].ToString()).ToString("HH:mm:ss"),
                    CardCode = (string)invoice["CardCode"],
                    CardName = (string)invoice["CardName"],
                    DocTotal = (decimal)invoice["DocTotal"],
                    VatSum = Math.Round(0.12m * (decimal)invoice["DocTotal"], 2),
                    TotalSinIVA = Math.Round((decimal)invoice["DocTotal"] - 0.12m * (decimal)invoice["DocTotal"], 2),
                    DocumentLines = GetDocumentLines(invoice["DocumentLines"])
                }).ToList();

                return Ok(invoices);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
            finally
            {
                if (!string.IsNullOrEmpty(sessionId))
                {
                    await _sapService.LogoutAsync(sessionId);
                }
            }
        }
        #endregion

        #region Api para obtener el tiempo de entrega
        [HttpGet("GetTimeDelivery")]
        public async Task<IActionResult> GetInvoicesTimeAsync(
            [FromQuery] string? numAtCard = null,
            [FromQuery] string? docNum = null,
            [FromQuery] string? docEntry = null)
        {
            if (string.IsNullOrEmpty(numAtCard) && string.IsNullOrEmpty(docNum) && string.IsNullOrEmpty(docEntry))
            {
                return BadRequest("Debe ingresar al menos un criterio de búsqueda.");
            }

            string filter = BuildFilter(numAtCard, docNum, docEntry);
            string sessionId = string.Empty;

            try
            {
                sessionId = await _sapService.LoginAsync();

                var rawData = await _sapService.GetInvoicesAsync(filter, sessionId);

                if (string.IsNullOrEmpty(rawData))
                {
                    return NotFound("No se recibieron datos de SAP.");
                }

                var result = JObject.Parse(rawData);
                var invoicesArray = result["value"];

                if (invoicesArray == null || !invoicesArray.Any())
                {
                    return NotFound("No se encontraron facturas con los criterios ingresados.");
                }

                var invoices = invoicesArray.Select(invoice =>
                {
                    DateTime docDate = DateTime.Parse(invoice["DocDate"].ToString());
                    DateTime docTime = DateTime.Parse(invoice["DocTime"].ToString());
                    DateTime startDateTime = docDate.Date.Add(docTime.TimeOfDay);

                    DateTime fechaEntrega = DateTime.Parse(invoice["U_FacFechaEntrega"].ToString());
                    DateTime horaEntrega = DateTime.Parse(invoice["U_FacHoraEntrega"].ToString());
                    DateTime endDateTime = fechaEntrega.Date.Add(horaEntrega.TimeOfDay);

                    TimeSpan timeDifference = endDateTime - startDateTime;

                    //string hoursAndMinutes = $"{(int)timeDifference.TotalHours}:{timeDifference.Minutes:D2}";
                    string hoursAndMinutes = $"{(int)timeDifference.TotalHours}";

                    return new
                    {
                        DocEntry = (int)invoice["DocEntry"],
                        NumAtCard = (string)invoice["NumAtCard"],
                        DocNum = (int)invoice["DocNum"],
                        DocDate = docDate.ToString("yyyy-MM-dd"),
                        DocTime = docTime.ToString("HH:mm:ss"),
                        U_FacFechaEntrega = fechaEntrega.ToString("yyyy-MM-dd"),
                        U_FacHoraEntrega = horaEntrega.ToString("HH:mm:ss"),
                        CardCode = (string)invoice["CardCode"],
                        CardName = (string)invoice["CardName"],
                        // Agregamos el cálculo de horas y minutos
                        TimeToDelivery = hoursAndMinutes
                    };
                }).ToList();

                return Ok(invoices);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
            finally
            {
                if (!string.IsNullOrEmpty(sessionId))
                {
                    await _sapService.LogoutAsync(sessionId);
                }
            }
        }
        #endregion

        #region Api separadas
        //#region Api para obtener el estado de la entrega
        //[HttpGet("GetOrderStatus")]
        //public async Task<IActionResult> GetOrderStatus([FromQuery] string numAtCard)
        //{
        //    if (string.IsNullOrEmpty(numAtCard))
        //    {
        //        return BadRequest("Debe proporcionar un número de factura (numAtCard).");
        //    }

        //    string filter = $"NumAtCard eq '{numAtCard}'";
        //    string sessionId = string.Empty;

        //    try
        //    {
        //        sessionId = await _sapService.LoginAsync();

        //        var rawData = await _sapService.GetInvoicesAsync(filter, sessionId);

        //        if (string.IsNullOrEmpty(rawData))
        //        {
        //            return NotFound("No se recibieron datos de SAP.");
        //        }

        //        var result = JObject.Parse(rawData);
        //        var invoice = result["value"]?.FirstOrDefault();

        //        if (invoice == null)
        //        {
        //            return Ok(new
        //            {
        //                NumAtCard = numAtCard,
        //                Status = new
        //                {
        //                    Solicitado = 0,
        //                    Programado = 0,
        //                    EnCamino = 0,
        //                    Entrega = 0
        //                }
        //            });
        //        }

        //        // Procesar datos de la factura
        //        DateTime docDate = DateTime.Parse(invoice["DocDate"].ToString());
        //        DateTime docTime = DateTime.Parse(invoice["DocTime"].ToString());
        //        DateTime creationDateTime = docDate.Date.Add(docTime.TimeOfDay);

        //        // Obtener valores de los campos U_FacFechaEntrega y U_FacHoraEntrega
        //        var fechaEntrega = invoice["U_FacFechaEntrega"]?.ToString();
        //        var horaEntrega = invoice["U_FacHoraEntrega"]?.ToString();

        //        // Validar si existen y son válidos
        //        bool entrega = !string.IsNullOrEmpty(fechaEntrega) && !string.IsNullOrEmpty(horaEntrega);

        //        // Calcular los otros estados
        //        int solicitado = 1;
        //        int programado = (DateTime.Now - creationDateTime).TotalHours >= 1 ? 1 : 0;
        //        int enCamino = (DateTime.Now - creationDateTime).TotalHours >= 2 ? 1 : 0;

        //        return Ok(new
        //        {
        //            NumAtCard = numAtCard,
        //            Status = new
        //            {
        //                Solicitado = solicitado,
        //                Programado = programado,
        //                EnCamino = enCamino,
        //                Entrega = entrega ? 1 : 0 // Solo será 1 si ambos campos son válidos
        //            }
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Error: {ex.Message}");
        //    }
        //    finally
        //    {
        //        if (!string.IsNullOrEmpty(sessionId))
        //        {
        //            await _sapService.LogoutAsync(sessionId);
        //        }
        //    }
        //}
        //#endregion

        //#region Api para obtener los datos del cliente
        //[HttpGet("GetClient")]
        //public async Task<IActionResult> GetClient(
        //[FromQuery] string? numAtCard = null,
        //[FromQuery] string? docNum = null,
        //[FromQuery] string? docEntry = null)
        //{
        //    if (string.IsNullOrEmpty(numAtCard) && string.IsNullOrEmpty(docNum) && string.IsNullOrEmpty(docEntry))
        //    {
        //        return BadRequest("Debe ingresar al menos un criterio de búsqueda.");
        //    }

        //    string filter = BuildFilter(numAtCard, docNum, docEntry);
        //    string sessionId = string.Empty;

        //    try
        //    {
        //        sessionId = await _sapService.LoginAsync();

        //        var rawData = await _sapService.GetInvoicesAsync(filter, sessionId);

        //        if (string.IsNullOrEmpty(rawData))
        //        {
        //            return NotFound("No se recibieron datos de SAP.");
        //        }

        //        var result = JObject.Parse(rawData);
        //        var invoicesArray = result["value"];

        //        if (invoicesArray == null || !invoicesArray.Any())
        //        {
        //            return NotFound("No se encontraron facturas con los criterios ingresados.");
        //        }

        //        var invoices = invoicesArray.Select(invoice => new
        //        {
        //            U_FacNom = (string)invoice["U_FacNom"],
        //            U_FacNit = (string)invoice["U_FacNit"],
        //            NumAtCard = (string)invoice["NumAtCard"],
        //            //U_FacFecha = DateTime.Parse(invoice["U_FacFecha"].ToString()).ToString("yyyy-MM-dd"),
        //            U_FacFecha = DateTime.Parse(invoice["U_FacFecha"].ToString())
        //            .ToString("dddd, d 'de' MMMM 'de' yyyy", new CultureInfo("es-ES")),
        //            U_Telefonos = (string)invoice["U_Telefonos"],
        //            Address = (string)invoice["Address"]


        //        }).ToList();

        //        return Ok(invoices);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Error: {ex.Message}");
        //    }
        //    finally
        //    {
        //        if (!string.IsNullOrEmpty(sessionId))
        //        {
        //            await _sapService.LogoutAsync(sessionId);
        //        }
        //    }
        //}
        //#endregion
        #endregion

        #region Api junta
        [HttpGet("GetOrderStatus")]
        public async Task<IActionResult> GetOrderStatus([FromQuery] string numAtCard)
        {
            if (string.IsNullOrEmpty(numAtCard))
            {
                return BadRequest("Debe proporcionar un número de factura (numAtCard).");
            }

            string filter = $"NumAtCard eq '{numAtCard}'";
            string sessionId = string.Empty;

            try
            {
                sessionId = await _sapService.LoginAsync();
                var rawData = await _sapService.GetInvoicesAsync(filter, sessionId);

                if (string.IsNullOrEmpty(rawData))
                {
                    return NotFound("No se recibieron datos de SAP.");
                }

                var result = JObject.Parse(rawData);
                var invoice = result["value"]?.FirstOrDefault();

                if (invoice == null)
                {
                    return Ok(new
                    {
                        NumAtCard = numAtCard,
                        Status = new
                        {
                            Solicitado = 0,
                            Programado = 0,
                            EnCamino = 0,
                            Entrega = 0
                        },
                        ClientInfo = "No se encontraron datos del cliente."
                    });
                }

                // Procesar datos del estado de entrega
                DateTime docDate = DateTime.Parse(invoice["DocDate"].ToString());
                DateTime docTime = DateTime.Parse(invoice["DocTime"].ToString());
                DateTime creationDateTime = docDate.Date.Add(docTime.TimeOfDay);

                var fechaEntrega = invoice["U_FacFechaEntrega"]?.ToString();
                var horaEntrega = invoice["U_FacHoraEntrega"]?.ToString();
                bool entrega = !string.IsNullOrEmpty(fechaEntrega) && !string.IsNullOrEmpty(horaEntrega);

                int solicitado = 1;
                int programado = (DateTime.Now - creationDateTime).TotalHours >= 1 ? 1 : 0;
                int enCamino = (DateTime.Now - creationDateTime).TotalHours >= 2 ? 1 : 0;

                // Procesar datos del cliente
                var clientInfo = new
                {
                    U_FacNom = (string)invoice["U_FacNom"],
                    U_FacNit = (string)invoice["U_FacNit"],
                    NumAtCard = (string)invoice["NumAtCard"],
                    U_FacFecha = DateTime.Parse(invoice["U_FacFecha"].ToString())
                        .ToString("dddd, d 'de' MMMM 'de' yyyy", new CultureInfo("es-ES")),
                    U_Telefonos = (string)invoice["U_Telefonos"],
                    Address = (string)invoice["Address"]
                };

                return Ok(new
                {
                    NumAtCard = numAtCard,
                    Status = new
                    {
                        Solicitado = solicitado,
                        Programado = programado,
                        EnCamino = enCamino,
                        Entrega = entrega ? 1 : 0
                    },
                    ClientInfo = clientInfo
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
            finally
            {
                if (!string.IsNullOrEmpty(sessionId))
                {
                    await _sapService.LogoutAsync(sessionId);
                }
            }
        }

            #endregion

        #region Api para actualizar el tiempo
            [HttpPost("UpdateTime")]
        public async Task<IActionResult> UpdateDeliveryDate(
            [FromQuery] string? numAtCard = null,
            [FromQuery] string? docNum = null,
            [FromQuery] string? docEntry = null)
        {
            if (string.IsNullOrEmpty(numAtCard) && string.IsNullOrEmpty(docNum) && string.IsNullOrEmpty(docEntry))
            {
                return BadRequest("Debe ingresar al menos un criterio de búsqueda.");
            }

            string filter = BuildFilter(numAtCard, docNum, docEntry);
            string sessionId = string.Empty;

            try
            {
                sessionId = await _sapService.LoginAsync();

                var rawData = await _sapService.GetInvoicesAsync(filter, sessionId);

                if (string.IsNullOrEmpty(rawData))
                {
                    return NotFound("No se recibió ningún dato de SAP.");
                }

                var result = JObject.Parse(rawData);
                var invoice = result["value"]?.FirstOrDefault();

                if (invoice == null)
                {
                    return NotFound("No se encontró ninguna factura con los criterios ingresados.");
                }

                int docEntryValue = (int)invoice["DocEntry"];

                var updateData = new JObject
                {
                    ["U_FacFechaEntrega"] = DateTime.UtcNow.ToString("yyyy-MM-dd"),
                    ["U_FacHoraEntrega"] = int.Parse(DateTime.Now.ToString("HHmm"))
                };

                bool updateSuccess = await _sapService.UpdateInvoiceAsync(docEntryValue, updateData, sessionId);

                if (!updateSuccess)
                {
                    return StatusCode(500, "No se pudo actualizar la fecha de entrega.");
                }

                return Ok("Fecha de entrega actualizada correctamente.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
            finally
            {
                if (!string.IsNullOrEmpty(sessionId))
                {
                    await _sapService.LogoutAsync(sessionId);
                }
            }
        }
        #endregion

        #region Fucniones
        private List<object> GetDocumentLines(JToken? linesToken)
        {
            if (linesToken == null)
            {
                return new List<object>();
            }

            return linesToken.Select(line => new
            {
                LineNum = (int)line["LineNum"],
                ItemCode = (string)line["ItemCode"],
                ItemDescription = line["ItemDescription"]?.ToString() ?? "(Sin descripción)",
                Quantity = (decimal)line["Quantity"],
                OpenCreQty = (decimal?)line["OpenCreQty"] ?? 0,
                Price = (decimal)line["Price"],
                LineTotal = (decimal)line["LineTotal"]
            }).Cast<object>().ToList();
        }

        private string BuildFilter(string? numAtCard, string? docNum, string? docEntry)
        {
            var filters = new List<string>();

            if (!string.IsNullOrEmpty(numAtCard))
            {
                filters.Add($"NumAtCard eq '{numAtCard}'");
            }
            if (!string.IsNullOrEmpty(docNum))
            {
                filters.Add($"DocNum eq {docNum}");
            }
            if (!string.IsNullOrEmpty(docEntry))
            {
                filters.Add($"DocEntry eq {docEntry}");
            }

            return string.Join(" or ", filters);
        }
        #endregion
    }
}
