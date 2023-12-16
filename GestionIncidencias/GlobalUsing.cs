global using System;
global using System.Data;
global using System.Security.Claims;
global using System.Diagnostics;
global using System.ComponentModel;
global using System.ComponentModel.DataAnnotations;
global using System.Net.Mail;

global using Microsoft.EntityFrameworkCore;
global using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
global using Microsoft.AspNetCore.Identity;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Authentication;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Authentication.Cookies;
global using Microsoft.AspNetCore.Mvc.Authorization;

global using ClosedXML.Excel;

global using GestionIncidencias;
global using GestionIncidencias.Entidades;
global using GestionIncidencias.Models;
global using GestionIncidencias.Servicios;
global using static GestionIncidencias.Servicios.IServicioCorreo;
global using static GestionIncidencias.Servicios.IServicioUsuarios;

