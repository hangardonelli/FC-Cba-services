#pragma warning disable IDE1006
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Core.Negocio.Predios;
using Core.Negocio.Canchas;
using Core.Negocio.Usuarios;
using Core.Negocio.HorariosPredios;
namespace Core.Negocio.Turnos
{
    public class TurnoFutbol
    {
        #region Propiedades públicas
        public int? Id { get; set; }
        public Predio? Predio { get; set; }
        public CanchaFutbol? Cancha { get; set; }
        public Usuario? Usuario { get; set; }
        public TiposPagoTurno? MetodoPago { get; set; }
        public TipoCesped? TipoCesped { get; set; }
        public int? Precio { get; set; }
        public DateOnly? Fecha { get; set; }
        public decimal? Hora
        {
            get => _Hora;
            set
            {
                if (value == null) throw new NullReferenceException("No se ha introducido un horario del turno");
                else if (value < 0 || value > (decimal)23.59) throw new ArgumentOutOfRangeException(nameof(Hora));
                else _Hora = value;
            }
        }
        public EstadoTurno? Estado { get; set; }
        public string? MensajeCancelacion { get; set; }

        /// <summary>
        /// Representa el código de verificación del turno
        /// </summary>
        public string? Codigo { get; set; }

        /// <summary>
        /// La opinión que cuenta la experiencia del usuario en el predio
        /// </summary>
        public string? Opinion { get; set; }

        public Calificaciones? Calificacion { get; set; }

        #endregion

        #region Propiedades privadas
        protected decimal? _Hora { get; set; }
        #endregion

        #region Metodos públicos

        /// <summary>
        /// Guarda en base de datos la calificación del turno de esta instancia
        /// </summary>
        public TurnoFutbolResponse Calificar()
        {
            TurnoFutbolResponse response = new() { AccionRealizada = AccionRealizada.MODIFICAR };
            try
            {
                if (!Id.HasValue) throw new Exception("No se ha inicializado el turno");
                if (!Calificacion.HasValue) throw new Exception("No se ha cargado una calificación para este turno");

                _ = Datos.Turnos.TurnoFutbol.Calificar(Id.Value, (int)Calificacion.Value, MensajeCancelacion);

                response.Contenido = this;
                response.Resultado = Tuneles.StatusCode.OK;
            }
            catch(Exception ex)
            {
                response.Resultado = Tuneles.StatusCode.ERROR;
                response.Mensaje = "Se ha producido un error al calificar el turno: " + ex.Message;
            }

            return response;
        }
        /// <summary>
        /// Envía a base de datos la modificación del turno actual
        /// </summary>
        public TurnoFutbolResponse Modificar()
        {
            TurnoFutbolResponse response = new TurnoFutbolResponse() { AccionRealizada = AccionRealizada.MODIFICAR};

            try
            {
                #region Chequeo nulls
                if (!Id.HasValue) throw new Exception("No se ha inicializado el turno");
                if (!Estado.HasValue) throw new Exception("No se pudo obtener el estado actual del turno");
                if (!MetodoPago.HasValue) throw new Exception("No se pudo obtener el metodo de pago del turno");
                if (!TipoCesped.HasValue) throw new Exception("No se pudo obtener el tipo de cesped del turno");
                if (!Hora.HasValue) throw new Exception("No se pudo obtener el horario del turno ");
                #endregion

                _ = Datos.Turnos.TurnoFutbol.Modificar(Id.Value, (int)Estado.Value, (int)MetodoPago.Value, (int)TipoCesped.Value, Hora.Value);
                response.Contenido = this;
                response.Resultado = Tuneles.StatusCode.OK;
            }
            catch(Exception ex)
            {
                response.Resultado = Tuneles.StatusCode.ERROR;
                response.Mensaje = "Hubo un error al modificar el turno: " + ex.Message;
            }
            return response;
        }
        public static TurnoFutbolResponse Obtener(DataRow row, Usuario usuario, List<CanchaFutbol> cacheCanchas, List<Predio> cachePredios)
        {
            TurnoFutbolResponse response = new() { AccionRealizada = AccionRealizada.VERIFICARDISPONIBILIDAD };
            try
            {
                #region Chequeo nulls
                if (row["id"] == null) throw new Exception("No se pudo cargar la identificación de un turno");
                if (row["Fecha"] == null) throw new Exception("No se pudo cargar la fecha de un turno");
                if (row["predio_id"] == null) throw new Exception("No se pudo cargar el identificador del predio de un turno");
                if (row["estado"] == null) throw new Exception("No se pudo cargar el estado de un turno");
                if (row["cancha_id"] == null) throw new Exception("No se pudo cargar el identificador de la cancha de un turno");
                if (row["Hora"] == null) throw new Exception("No se pudo cargar la hora de un turno");
                if (row["precio"] == null) throw new Exception("No se pudo cargar el precio de un turno");
                if (row["codigo"] == null) throw new Exception("No se pudo establecer el estado del código del turno");
                #endregion

                #region Chequeo formato
                if (!Int32.TryParse(row["id"].ToString(), out int r_turno_id)) throw new FormatException("El identificador del turno no tiene el formato correcto");
                DateOnly r_turno_fecha = DateOnly.FromDateTime(Convert.ToDateTime(row["Fecha"]));
                if (!Int32.TryParse(row["predio_id"].ToString(), out int r_turno_predio_id)) throw new FormatException("El identificador del predio del turno no tiene el formato correcto");
                if (!Int32.TryParse(row["estado"].ToString(), out int r_turno_estado)) throw new FormatException("El estado del turno no tiene el formato correcto");
                if (row["cancha_id"].ToString() == null) throw new Exception("No se ha cargado el identificador de la cancha del turno");
                if (!Decimal.TryParse(row["Hora"].ToString(), out decimal r_turno_hora)) throw new FormatException("La hora de un turno no tiene el formato correcto");
                if (!Int32.TryParse(row["precio"].ToString(), out int r_turno_precio)) throw new FormatException("El precio del turno no tiene el formato correcto");

                #endregion
                Predio? predio = cachePredios.FirstOrDefault(p => p.Id == r_turno_predio_id);
                CanchaFutbol? cancha = cacheCanchas.FirstOrDefault(c => c.Id == row["cancha_id"].ToString());

                response.Contenido = new TurnoFutbol()
                {
                    Id = r_turno_id,
                    Fecha = r_turno_fecha,
                    Predio = predio,
                    Cancha = cancha,
                    Hora = r_turno_hora,
                    Usuario = usuario,
                    Precio = r_turno_precio,
                    Codigo = row["codigo"].ToString()
                };

                response.Resultado = Tuneles.StatusCode.OK;
            
            }
            catch (Exception ex)
            {
                response.Resultado = Tuneles.StatusCode.ERROR;
                response.Mensaje = ex.Message;
            }

            return response;
        }
        public static TurnoFutbolListaResponse ObtenerDisponibles(DateOnly fecha, decimal horaDesde, decimal horaHasta, List<Predio>? cachePredios = null, List<CanchaFutbol>? cacheCanchas = null)
        {
            TurnoFutbolListaResponse response = new() { AccionRealizada = AccionRealizada.DISPONIBLES };
            try
            {
                if (horaDesde < 0 || horaHasta > (decimal)23.59) throw new Exception("El horario introducido no es válido");

                #region Obtención de predios por cache
                if (cachePredios == null)
                {
                    var predioResponse = Predio.ObtenerTodos();
                    if (predioResponse.Resultado == Tuneles.StatusCode.OK && predioResponse.Contenido != null)
                        cachePredios = predioResponse.Contenido;
                    else throw new Exception(predioResponse.Mensaje ?? "Se produjo un error desconocido al obtener los predios de FC - Cordoba");
            
                }
                #endregion

                #region Obtención de canchas por cache
                if (cacheCanchas == null)
                {
                    cacheCanchas = new List<CanchaFutbol>();
                    foreach (Predio predio in cachePredios)
                    {
                        var obtenerCanchaPredioResponse = predio.ObtenerCanchasNoArbol();
                        if (obtenerCanchaPredioResponse.Resultado == Tuneles.StatusCode.OK && obtenerCanchaPredioResponse.Contenido != null)
                            cacheCanchas.AddRange(obtenerCanchaPredioResponse.Contenido);
                        else throw new Exception(obtenerCanchaPredioResponse.Mensaje ?? "Se ha producido un error desconocido al obtener las canchas de un predio");
                    }
                }
                #endregion

                #region Obtención turnos reservados y validación
                var reservadosResponse = ObtenerReservados(fecha, horaDesde, horaHasta, cachePredios);
                if (reservadosResponse.Resultado != Tuneles.StatusCode.OK || reservadosResponse.Contenido == null)
                    throw new Exception(reservadosResponse.Mensaje ?? "Se ha producido un error desconocido al obtener los turnos reservados");
                #endregion

                List<TurnoFutbol> turnosDisponibles = new List<TurnoFutbol>();
                foreach (CanchaFutbol cancha in cacheCanchas)
                {
                    #region Chequeo nulls de cada cancha obtenida
                    if (cancha == null) throw new Exception("No se ha inicializado la cancha de un predio");
                    if (cancha.Predio == null) throw new Exception("No se ha inicializado predio de una cancha");
                    if (cancha.Predio.Horarios == null) throw new Exception("No se ha inicializado el horario un predio");
                    #endregion

                    foreach (HorarioPredio horario in cancha.Predio.Horarios)
                    {
                        #region Chequeo nulls horario
                        if (!horario.Hora.HasValue) throw new Exception("No se inicializó el horario de un predio");
                        if (!horario.DiaSemana.HasValue) throw new Exception("No se inicializó el dia de la semana de un predio");
                        if (horario.Predio == null) throw new Exception("No se inicializó el predio de un horario");
                        if (horario.Predio.Id == null) throw new Exception("No se inicializó el identificador del predio de un horario");
                        #endregion

                        #region Agregar turno a la lista de turnos disponibles si corresponde
                        bool correspondeHorario = (horario.Hora >= horaDesde) && (horario.Hora <= horaHasta);
                        bool correspondeDia = horario.DiaSemana == fecha.DayOfWeek;
                       
                        bool estaReservado = reservadosResponse.Contenido.Exists(
                                                                        t => t.Hora == horario.Hora &&
                                                                             t.Fecha == fecha &&
                                                                             t.Cancha.Id == cancha.Id &&
                                                                             t.Predio.Id == cancha.Predio.Id
                                                                        );
                        if (correspondeHorario && correspondeDia && !estaReservado && cancha.Habilitada && cancha.Predio.Habilitado)
                        {
                            TurnoFutbol turnoDisponible = new();
                            turnoDisponible.Predio = horario.Predio;
                            turnoDisponible.Cancha = cancha;
                            turnoDisponible.Fecha = fecha;
                            turnoDisponible.Hora = horario.Hora;
                            turnoDisponible.Precio = cancha.Precio;
                            turnoDisponible.TipoCesped = cancha.TipoCesped;
                            turnosDisponibles.Add(turnoDisponible);
                        }
                    }
                    #endregion
                }

                response.Resultado = Tuneles.StatusCode.OK;
                response.Contenido = turnosDisponibles;

            }

            catch (Exception ex)
            {
                response.Resultado = Tuneles.StatusCode.ERROR;
                response.Mensaje = ex.Message;
            }

            return response;
        }
        public static TurnoFutbolListaResponse ObtenerReservados(DateOnly fecha, decimal horaDesde, decimal horaHasta, List<Predio>? cachePredios = null, List<CanchaFutbol>? cacheCanchasFutbol = null)
        {
            TurnoFutbolListaResponse response = new() { AccionRealizada = AccionRealizada.VERIFICARDISPONIBILIDAD };
            try
            {
                #region Asignación de caché
                if (cachePredios == null) cachePredios = new();
                if (cacheCanchasFutbol == null) cacheCanchasFutbol = new();
                #endregion

                List<TurnoFutbol> turnosReservados = new();

                #region Validación datos de entrada
                if (horaDesde < 0 && horaDesde > 24) throw new ArgumentOutOfRangeException(nameof(horaDesde));
                if (horaHasta < 0 && horaHasta > 24) throw new ArgumentOutOfRangeException(nameof(horaDesde));
                if (fecha < DateOnly.FromDateTime(DateTime.Now)) throw new ArgumentOutOfRangeException(nameof(fecha));
                #endregion


                DataTable datos = Datos.Turnos.TurnoFutbol.ObtenerTurnos(fecha, horaDesde, horaHasta);


                foreach (DataRow row in datos.Rows)
                {
                    #region Chequeo nulls
                    if (row["turno_id"] == null) throw new Exception("No se pudo cargar la identificación de un turno");
                    if (row["turno_fecha"] == null) throw new Exception("No se pudo cargar la fecha de un turno");
                    if (row["turno_predio_id"] == null) throw new Exception("No se pudo cargar el identificador del predio de un turno");
                    if (row["turno_estado"] == null) throw new Exception("No se pudo cargar el estado de un turno");
                    if (row["turno_cancha_id"] == null) throw new Exception("no se pudo cargar el identificador de la cancha de un turno");
                    if (row["turno_hora"] == null) throw new Exception("no se pudo cargar la hora de un turno");
                    if (row["codigo"] == null) throw new Exception("no se pudo cargar el código de un turno");

                    #endregion

                    #region Chequeo formato
                    if (!Int32.TryParse(row["turno_id"].ToString(), out int r_turno_id)) throw new FormatException("El identificador del turno no tiene el formato correcto");
                    DateOnly r_turno_fecha = DateOnly.FromDateTime(Convert.ToDateTime(row["turno_fecha"]));
                    if (!Int32.TryParse(row["turno_predio_id"].ToString(), out int r_turno_predio_id)) throw new FormatException("El identificador del predio del turno no tiene el formato correcto");
                    if (!Int32.TryParse(row["turno_estado"].ToString(), out int r_turno_estado)) throw new FormatException("El estado del turno no tiene el formato correcto");
                    if (row["turno_cancha_id"].ToString() == null) throw new Exception("No se ha cargado el identificador de la cancha del turno");
                    if (!Decimal.TryParse(row["turno_hora"].ToString(), out decimal r_turno_hora)) throw new FormatException("La hora de un turno no tiene el formato correcto");
                    #endregion

                    TurnoFutbol reservaActual = new();
                    Predio? predioActual = cachePredios.FirstOrDefault(p => p.Id == r_turno_predio_id);
                    CanchaFutbol? canchaActual = cacheCanchasFutbol.FirstOrDefault(p => p.Id == row["turno_cancha_id"].ToString());

                    //Si no está en cache: lo buscamos en base de datos y lo agregamos a la cache:
                    if (predioActual == null)
                    {
                        PredioResponse predioConsulta = Predio.Obtener(r_turno_predio_id);
                        if (predioConsulta.Resultado == Tuneles.StatusCode.OK && predioConsulta.Contenido != null)
                            predioActual = predioConsulta.Contenido;
                        else throw new Exception(predioConsulta.Mensaje ?? "No se pudo obtener el predio");

                        cachePredios.Add(predioActual);
                    }



                    //Si no está en cache: lo buscamos en base de datos y lo agregamos a la cache:
                    if (canchaActual == null)
                    {
                        CanchaResponse canchaConsulta = CanchaFutbol.Obtener(row["turno_cancha_id"].ToString(), cachePredios.First(p => p.Id == r_turno_predio_id));
                        if (canchaConsulta.Resultado == Tuneles.StatusCode.OK && canchaConsulta.Contenido != null)
                            canchaActual = canchaConsulta.Contenido;
                        else throw new Exception(canchaConsulta.Mensaje ?? "No se pudo obtener la cancha");

                        cacheCanchasFutbol.Add(canchaActual);
                    }

                    reservaActual.Id = r_turno_id;
                    reservaActual.Hora = r_turno_hora;
                    reservaActual.Fecha = r_turno_fecha;
                    reservaActual.Predio = predioActual;
                    reservaActual.Cancha = canchaActual;
                    reservaActual.Precio = canchaActual.Precio;
                    reservaActual.TipoCesped = canchaActual.TipoCesped;
                    reservaActual.Codigo = row["codigo"].ToString();

                    turnosReservados.Add(reservaActual);
                }

                response.Contenido = turnosReservados;
                response.Resultado = Tuneles.StatusCode.OK;
            }
            catch (ArgumentOutOfRangeException ex)
            {
                response.Resultado = Tuneles.StatusCode.ERROR;
                switch (ex.ParamName)
                {
                    case nameof(fecha):
                        response.Mensaje = "La fecha introducida no es valida";
                        response.Mensaje = "DATE_OUT_OF_RANGE";
                        break;
                    case nameof(horaDesde):
                        response.Mensaje = "La hora introducida no es valida";
                        response.CodigoError = "HOUR_OUT_OF_RANGE";
                        break;
                    case nameof(horaHasta):
                        response.Mensaje = "La hora introducida no es valida";
                        response.CodigoError = "HOUR_OUT_OF_RANGE";
                        break;
                    default:
                        response.Mensaje = $"Argumento fuera de rango no controlado ({ex.ParamName})";
                        response.CodigoError = "NOT_CONTROLLED_ARGUMENT_OUT_OF_RANGE";
                        break;
                }

            }
            catch (Exception ex)
            {
                response.Resultado = Tuneles.StatusCode.ERROR;
                response.Mensaje = ex.Message;
            }

            return response;
        }

        /// <summary>
        /// Denota si un turno en determinado predio, cancha y tiempo está disponiblee
        /// </summary>
        public static TurnoFutbolDisponibilidadResponse VerificarDisponibilidad(DateOnly fecha, decimal hora, Predio predio, CanchaFutbol cancha)
        {

            TurnoFutbolDisponibilidadResponse response = new() { AccionRealizada = AccionRealizada.VERIFICARDISPONIBILIDAD };
            try
            {
                #region Chequeo nulos
                if (cancha == null) throw new Exception("La cancha ingresada no se cargó");
                if (cancha.Id == null) throw new Exception("El identificador de la cancha no se cargó");
                if (predio == null) throw new Exception("El predio ingresado no se cargó");
                if (predio.Id == null) throw new Exception("El identificador del predio no se cargó");
                #endregion

                #region Obtención de canchas ocupadas
                List<string> canchasHijas = ObtenerCanchasHijas(cancha);
                List<string> canchasPadres = ObtenerCanchasPadres(cancha);
                List<string> canchasOcupadas = new() { cancha.Id };

                canchasOcupadas = canchasHijas.Union(canchasPadres).ToList();
                string canchasJoin = String.Join('|', canchasOcupadas.ToArray());
                #endregion

                DataTable canchasReservadasDatos = Datos.Turnos.TurnoFutbol.ObtenerReservados(canchasJoin, predio.Id.Value, fecha, hora);

                if (canchasReservadasDatos == null) throw new Exception("No se pudieron cargar los turnos reservados correctamente");

                List<string> canchasReservadas = new();
                foreach (DataRow row in canchasReservadasDatos.Rows)
                {
                    if (row != null && row["id_cancha"] != null)
                        canchasReservadas.Add(row["id_cancha"].ToString());
                    else throw new Exception("No se pudo inicializar el id de una cancha reservada");
                }

                response.CanchasReservadas = canchasReservadas;
                response.CanchasDisponibles = canchasOcupadas.Where(o => !canchasReservadas.Any(r => r == o)).ToList();
                response.Contenido = response.CanchasDisponibles.Exists(d => d == cancha.Id);
                response.Resultado = Tuneles.StatusCode.OK;

            }
            catch (Exception ex)
            {
                response.Resultado = Tuneles.StatusCode.ERROR;
                response.Mensaje = ex.Message;
            }

            return response;
        }
        /// <summary>
        /// Alquila la cancha actual
        /// </summary>
        public TurnoFutbolResponse Crear()
        {
            TurnoFutbolResponse response = new() { AccionRealizada = AccionRealizada.CREAR };

            try
            {
                #region Chequear nulls
                string campoActual;
                campoActual = "Predio";
                if (Predio == null)
                    throw new NullReferenceException("Campo: " + campoActual);
                campoActual = "Identificador del predio";
                if (!Predio.Id.HasValue)
                    throw new NullReferenceException("Campo: " + campoActual);
                campoActual = "Cancha";
                if (Cancha == null)
                    throw new NullReferenceException("Campo: " + campoActual);
                campoActual = "Identificador de la cancha";
                if (Cancha.Id == null)
                    throw new NullReferenceException("Campo: " + campoActual);
                campoActual = "Metodo de pago";
                if (!MetodoPago.HasValue)
                    throw new NullReferenceException("Campo: " + campoActual);
                campoActual = "Tipo de cesped";
                if (!TipoCesped.HasValue)
                    throw new NullReferenceException("Campo: " + campoActual);
                campoActual = "Precio";
                if (!Precio.HasValue)
                    throw new NullReferenceException("Campo: " + campoActual);
                campoActual = "Fecha";
                if (!Fecha.HasValue)
                    throw new NullReferenceException("Campo: " + campoActual);
                campoActual = "Hora";
                if (!Hora.HasValue)
                    throw new NullReferenceException("Campo: " + campoActual);
                campoActual = "Estado";
                if (!Estado.HasValue)
                    throw new NullReferenceException("Campo: " + campoActual);
                campoActual = "Usuario";
                if (Usuario == null)
                    throw new NullReferenceException("Campo: " + campoActual);
                campoActual = "Identificador del usuario";
                if (!Usuario.Id.HasValue)
                    throw new NullReferenceException("Campo: " + campoActual);

                #endregion

                Datos.Turnos.TurnoFutbol.Crear(Fecha.Value.ToDateTime(TimeOnly.FromDateTime(DateTime.Now)), Hora.Value, Predio.Id.Value, (short)Estado, (short)MetodoPago, Cancha.Id, (int)TipoCesped, Precio.Value, Usuario.Id.Value);
                response.Resultado = Tuneles.StatusCode.OK;
            }
            catch (ArgumentOutOfRangeException ex)
            {
                response.Resultado = Tuneles.StatusCode.ERROR;
                if (ex.ParamName == nameof(Hora)) response.Mensaje = "El horario introducido no es válido";
                else response.Mensaje = ex.Message + "--" + ex.ParamName;

            }
            catch (NullReferenceException ex)
            {
                response.Resultado = Tuneles.StatusCode.ERROR;
                response.Mensaje = "No se cargó completamente un campo. " + ex.Message;

            }
            catch (Exception ex)
            {
                response.Resultado = Tuneles.StatusCode.ERROR;
                response.Mensaje = ex.Message;
            }

            return response;
        }
        #endregion

        #region Metodos privados

    
        /// <summary>
        /// Obtiene las canchas padres de una cancha
        /// </summary>
        private static List<string> ObtenerCanchasPadres(CanchaFutbol cancha) => (cancha.Id == null) ? throw new Exception("No se ha inicializado la cancha de fútbol correctamente") : cancha?.Id.Split('.').ToList();
        private static List<string> ObtenerCanchasHijas(CanchaFutbol cancha, List<CanchaFutbol> canchasObtenidas = null)
        {

            if (canchasObtenidas == null) canchasObtenidas = new List<CanchaFutbol>();

            foreach (CanchaFutbol _cancha in cancha.Divisiones)
            {
                if (_cancha.Id == null) throw new Exception("No se ha podido obtener el identificador de la cancha durante la verificación del turno");
                canchasObtenidas.Add(_cancha);
                ObtenerCanchasHijas(_cancha, canchasObtenidas);
            }

            return canchasObtenidas.Select(c => c.Id.ToString()).ToList();
        }
        #endregion

    }
}




