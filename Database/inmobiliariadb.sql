-- phpMyAdmin SQL Dump
-- version 5.1.0
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 26-09-2025 a las 14:42:04
-- Versión del servidor: 10.4.19-MariaDB
-- Versión de PHP: 8.0.6

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de datos: `inmobiliariadb`
--

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `contrato`
--

CREATE TABLE `contrato` (
  `Id` int(11) NOT NULL,
  `InmuebleId` int(11) NOT NULL,
  `InquilinoId` int(11) NOT NULL,
  `FechaInicio` date NOT NULL,
  `FechaFin` date DEFAULT NULL,
  `Monto` decimal(12,2) NOT NULL,
  `Periodicidad` varchar(50) DEFAULT 'Mensual',
  `Estado` varchar(50) DEFAULT 'Vigente',
  `Observaciones` text DEFAULT NULL,
  `UsuarioCreacionContrato` varchar(100) DEFAULT NULL,
  `FechaCreacionContrato` datetime DEFAULT NULL,
  `UsuarioAnulacionContrato` varchar(100) DEFAULT NULL,
  `FechaAnulacionContrato` datetime DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Volcado de datos para la tabla `contrato`
--

INSERT INTO `contrato` (`Id`, `InmuebleId`, `InquilinoId`, `FechaInicio`, `FechaFin`, `Monto`, `Periodicidad`, `Estado`, `Observaciones`, `UsuarioCreacionContrato`, `FechaCreacionContrato`, `UsuarioAnulacionContrato`, `FechaAnulacionContrato`) VALUES
(33, 2, 18, '2025-09-24', '2025-10-24', '300000.00', 'Mensual', 'Anulado', NULL, 'Andrea', '2025-09-25 11:53:12', 'Daniela', '2025-09-25 12:49:34'),
(35, 53, 19, '2025-09-24', '2026-09-24', '440000.00', 'Anual', 'Vigente', NULL, 'James', '2025-09-25 11:53:12', NULL, NULL),
(36, 19, 14, '2025-09-24', '2025-12-24', '800000.00', 'Trimestral', 'Vigente', NULL, 'Daniela', '2025-09-25 11:53:12', NULL, NULL),
(38, 34, 14, '2025-09-24', '2025-12-24', '430000.00', 'Trimestral', 'Anulado', NULL, 'Carla', '2025-09-25 11:53:12', 'James', '2025-09-25 12:45:18'),
(39, 17, 19, '2025-06-01', '2025-12-01', '800000.00', 'Semestral', 'Vigente', NULL, 'Carla', '2025-09-25 11:53:12', NULL, NULL),
(40, 34, 20, '2025-09-25', '2025-12-25', '800000.00', 'Trimestral', 'Vigente', NULL, 'Exequiel', '2025-09-25 11:53:12', NULL, NULL),
(41, 29, 18, '2025-06-25', '2025-12-25', '6.00', 'Semestral', 'Anulado', NULL, 'Daniela', '2025-09-25 11:53:12', 'Exequiel', '2025-09-25 12:42:18'),
(42, 2, 14, '2025-09-25', '2025-10-25', '430000.00', 'Mensual', 'Anulado', NULL, 'Exequiel', '2025-09-25 12:32:47', 'Daniela', '2025-09-25 12:44:34'),
(43, 28, 18, '2025-09-01', '2026-02-01', '900000.00', 'Semestral', 'Anulado', NULL, 'Carla', '2025-09-25 13:16:43', 'Andrea', '2025-09-25 16:29:32'),
(44, 29, 17, '2025-09-25', '2026-03-25', '900000.00', 'Semestral', 'Vigente', NULL, 'Daniela', '2025-09-25 16:29:01', NULL, NULL);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inmueble`
--

CREATE TABLE `inmueble` (
  `Id` int(11) NOT NULL,
  `Direccion` varchar(255) NOT NULL,
  `Ambientes` int(11) DEFAULT NULL,
  `Superficie` decimal(10,2) DEFAULT NULL,
  `Precio` decimal(12,2) DEFAULT NULL,
  `PropietarioId` int(11) NOT NULL,
  `Estado` varchar(50) DEFAULT 'Disponible',
  `Observaciones` text DEFAULT NULL,
  `TipoId` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Volcado de datos para la tabla `inmueble`
--

INSERT INTO `inmueble` (`Id`, `Direccion`, `Ambientes`, `Superficie`, `Precio`, `PropietarioId`, `Estado`, `Observaciones`, `TipoId`) VALUES
(2, 'Sarmiento 2903', 1, '230.00', '32000.00', 1, 'Disponible', 'Ideal 2 personas, patio chico', 2),
(5, 'Sarmiento 2892', 2, '320.00', '39000.00', 5, 'Disponible', 'Grande', 3),
(17, 'Calle Falsa 1238', 3, '75.50', '120000.00', 1, 'Alquilado', 'Luminoso y céntrico', 4),
(19, 'Calle Libertad 45', 2, '60.00', '95000.00', 6, 'Alquilado', 'Recientemente remodelado', 4),
(20, 'Boulevard Central 100', 4, '120.00', '180000.00', 7, 'Alquilado', 'Ideal para consultorio', 5),
(28, 'Av. Libertador 1234', 3, '85.50', '120000.00', 1, 'Disponible', 'Departamento con balcón y vista al río', 4),
(29, 'Calle Falsa 742', 4, '120.00', '150000.00', 8, 'Alquilado', 'Casa familiar con jardín y garage', 6),
(30, 'Av. Corrientes 2000', 1, '60.00', '80000.00', 6, 'Disponible', 'Local comercial en esquina, alto tránsito', 7),
(31, 'Calle Córdoba 1500', 2, '70.00', '95000.00', 7, 'Disponible', 'Departamento amoblado, ideal estudiantes', 4),
(34, 'Avenida Lafinur 1240', 1, '1200.00', '43000.00', 1, 'Alquilado', 'Funcional y comodo', 2),
(53, 'Carlos Paz 200', 1, '3200.00', '850000.00', 1, 'Alquilado', 'Amplio y con Balcon', 3);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inquilino`
--

CREATE TABLE `inquilino` (
  `Id` int(11) NOT NULL,
  `DNI` varchar(20) NOT NULL,
  `Nombre` varchar(100) NOT NULL,
  `Apellido` varchar(100) NOT NULL,
  `Telefono` varchar(50) DEFAULT NULL,
  `Email` varchar(100) DEFAULT NULL,
  `Direccion` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Volcado de datos para la tabla `inquilino`
--

INSERT INTO `inquilino` (`Id`, `DNI`, `Nombre`, `Apellido`, `Telefono`, `Email`, `Direccion`) VALUES
(3, '32116321', 'Anas', 'López Juris', '555-1234', 'ana.lopez@email.com', 'Calle Primavera 1235'),
(14, '23456789', 'Luis', 'Martínez', '555-5678', 'luis.martinez@email.com', 'Avenida Central 456'),
(17, '34567890', 'María', 'Gómez', '555-9012', 'maria.gomez@email.com', 'Calle Sol 789'),
(18, '45678901', 'Carlos', 'Ramírez', '555-3456', 'carlos.ramirez@email.com', 'Boulevard Norte 321'),
(19, '56789012', 'Sofía', 'Fernández', '555-7890', 'sofia.fernandez@email.com', 'Calle Luna 654'),
(20, '26832921', 'Carlos', 'Perez', '2664332211', 'carlosp@gmail.com', 'Bolivar 1230');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `pago`
--

CREATE TABLE `pago` (
  `Id` int(11) NOT NULL,
  `ContratoId` int(11) NOT NULL,
  `Fecha` date NOT NULL,
  `Importe` decimal(12,2) NOT NULL,
  `MedioPago` varchar(50) DEFAULT NULL,
  `Observaciones` text DEFAULT NULL,
  `UsuarioCreacionPago` varchar(100) NOT NULL,
  `FechaCreacionPago` datetime NOT NULL DEFAULT current_timestamp(),
  `UsuarioAnulacionPago` varchar(100) DEFAULT NULL,
  `FechaAnulacionPago` datetime DEFAULT NULL,
  `Estado` varchar(20) NOT NULL DEFAULT 'Activo'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Volcado de datos para la tabla `pago`
--

INSERT INTO `pago` (`Id`, `ContratoId`, `Fecha`, `Importe`, `MedioPago`, `Observaciones`, `UsuarioCreacionPago`, `FechaCreacionPago`, `UsuarioAnulacionPago`, `FechaAnulacionPago`, `Estado`) VALUES
(33, 40, '1212-12-12', '123.00', 'Transferencia', 'Transferencia BNA', 'Exequiel', '2025-09-25 15:14:55', 'Daniela', '2025-09-25 15:43:11', 'Activo'),
(34, 40, '2025-09-25', '300000.00', 'Tarjeta', NULL, 'Exequiel', '2025-09-25 15:44:06', NULL, NULL, 'Activo'),
(35, 36, '1212-12-12', '12.00', 'Efectivo', NULL, 'Exequiel', '2025-09-25 15:44:28', 'Exequiel', '2025-09-25 15:44:34', 'Anulado'),
(36, 43, '2025-09-12', '400000.00', 'Transferencia', NULL, 'Exequiel', '2025-09-25 16:09:32', NULL, NULL, 'Activo');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `propietario`
--

CREATE TABLE `propietario` (
  `Id` int(11) NOT NULL,
  `DNI` varchar(20) NOT NULL,
  `Nombre` varchar(100) NOT NULL,
  `Apellido` varchar(100) NOT NULL,
  `Telefono` varchar(50) DEFAULT NULL,
  `Email` varchar(100) DEFAULT NULL,
  `Direccion` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Volcado de datos para la tabla `propietario`
--

INSERT INTO `propietario` (`Id`, `DNI`, `Nombre`, `Apellido`, `Telefono`, `Email`, `Direccion`) VALUES
(1, '28789404', 'Leonardo', 'Rivero', '2664642996', 'leonardo.rivero@email.com.ar', 'Calle Falsa 12345'),
(5, '294424242', 'Cliff', ' Burton', '2664255432', 'cliff.burton@email.com', 'Avenida Siempre Viva 742'),
(6, '12345678', 'Juan', 'Pérez', '1234567890', 'juan.perez@email.com', 'Calle Roble 101'),
(7, '23456789', 'María', 'Gómez', '0987654321', 'maria.gomez@email.com', 'Avenida Olmos 202'),
(8, '34567890', 'Carlos', 'Ramírez', '1122334455', 'carlos.ramirez@email.com', 'Boulevard Pino 303'),
(9, '28323123', 'Facundo', 'Torres', '2665223311', 'facu_t_@gmail.com', 'Falucho 1200');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `tipoinmueble`
--

CREATE TABLE `tipoinmueble` (
  `Id` int(11) NOT NULL,
  `Descripcion` varchar(50) NOT NULL,
  `Estado` enum('Activo','Inactivo') DEFAULT 'Activo',
  `FechaCreacion` timestamp NOT NULL DEFAULT current_timestamp(),
  `FechaModificacion` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Volcado de datos para la tabla `tipoinmueble`
--

INSERT INTO `tipoinmueble` (`Id`, `Descripcion`, `Estado`, `FechaCreacion`, `FechaModificacion`) VALUES
(1, 'Departamento 1 ambiente', 'Activo', '2025-09-21 03:12:51', '2025-09-23 14:21:28'),
(2, 'Monoambiente ', 'Activo', '2025-09-21 03:46:23', '2025-09-21 03:46:23'),
(3, 'Duplex', 'Activo', '2025-09-21 03:46:23', '2025-09-21 03:46:23'),
(4, 'Departamento', 'Activo', '2025-09-21 03:46:23', '2025-09-21 03:46:23'),
(5, 'Oficina', 'Activo', '2025-09-21 03:46:23', '2025-09-21 03:46:23'),
(6, 'Casa', 'Activo', '2025-09-21 03:46:23', '2025-09-21 03:46:23'),
(7, 'Local', 'Activo', '2025-09-21 03:46:23', '2025-09-21 21:37:37');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `usuario`
--

CREATE TABLE `usuario` (
  `Id` int(11) NOT NULL,
  `Nombre` varchar(50) NOT NULL,
  `Email` varchar(100) NOT NULL,
  `PasswordHash` varchar(255) NOT NULL,
  `Avatar` varchar(255) DEFAULT NULL,
  `Rol` int(11) NOT NULL,
  `Activo` tinyint(1) NOT NULL DEFAULT 1,
  `FechaCreacion` datetime NOT NULL DEFAULT current_timestamp(),
  `FechaUltimoLogin` datetime DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Volcado de datos para la tabla `usuario`
--

INSERT INTO `usuario` (`Id`, `Nombre`, `Email`, `PasswordHash`, `Avatar`, `Rol`, `Activo`, `FechaCreacion`, `FechaUltimoLogin`) VALUES
(8, 'Exequiel', 'leoexe@gmail.com', '$2a$11$mO1EzbgLJvn15vvc1RUU5.FQxjwCQ2LLsIHszxeiUTdg0UBhpgeJy', '4f5f084d-a572-435e-a6b6-8bff6a3e4078.jpg', 1, 1, '2025-09-22 15:57:32', '2025-09-26 09:32:06'),
(9, 'Daniela', 'dani_ela@gmail.com', '$2a$11$xWxrQn.HvFRRepUx/PAbZuPdG6qDAVAzsLnD6z2c/Yk2l97dqyV1y', '7eb8fe1e-fc87-4fb4-9ca4-92cfcb8e85e5.jpg', 2, 1, '2025-09-23 09:01:00', '2025-09-26 09:19:35'),
(10, 'James ', 'james_hetfield@gmail.com', '$2a$11$OZlvOg8dezfTRAqzAZbhbOt2pbHmVwTMXGRH5PNzNlQ4zhoEVIKxq', '79492e9a-b343-4037-adea-cde4629d7a56.png', 2, 1, '2025-09-23 09:01:31', '2025-09-23 12:21:50'),
(11, 'Andrea', 'andrea.fernandez@gmail.com', '$2a$11$eJ1zEgGW3UId8bQ0HecZLuRIcqJKiNIsB/3NOvMm9uXeqilLlrfeC', '006b4cdd-5481-47c1-a6fe-a40da7f02e42.jpg', 2, 1, '2025-09-23 10:56:53', '2025-09-25 16:29:14'),
(12, 'Carla', 'carlaB@gmail.com', '$2a$11$/LVb0q5zLVbrsu4Z/HSAZemvkMRMxGUfqwQs8FOjX/HHtnhmJcLD2', '1e7bf46c-65cd-4417-bb29-8b1f7e753942.jpeg', 2, 1, '2025-09-23 13:40:48', '2025-09-25 13:15:48');

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `contrato`
--
ALTER TABLE `contrato`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `fk_contrato_inmueble` (`InmuebleId`),
  ADD KEY `fk_contrato_inquilino` (`InquilinoId`);

--
-- Indices de la tabla `inmueble`
--
ALTER TABLE `inmueble`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `fk_inmueble_prop` (`PropietarioId`),
  ADD KEY `FK_Inmueble_TipoInmueble` (`TipoId`);

--
-- Indices de la tabla `inquilino`
--
ALTER TABLE `inquilino`
  ADD PRIMARY KEY (`Id`),
  ADD UNIQUE KEY `DNI` (`DNI`);

--
-- Indices de la tabla `pago`
--
ALTER TABLE `pago`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `fk_pago_contrato` (`ContratoId`);

--
-- Indices de la tabla `propietario`
--
ALTER TABLE `propietario`
  ADD PRIMARY KEY (`Id`),
  ADD UNIQUE KEY `DNI` (`DNI`);

--
-- Indices de la tabla `tipoinmueble`
--
ALTER TABLE `tipoinmueble`
  ADD PRIMARY KEY (`Id`),
  ADD UNIQUE KEY `Descripcion` (`Descripcion`);

--
-- Indices de la tabla `usuario`
--
ALTER TABLE `usuario`
  ADD PRIMARY KEY (`Id`),
  ADD UNIQUE KEY `Email` (`Email`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `contrato`
--
ALTER TABLE `contrato`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=46;

--
-- AUTO_INCREMENT de la tabla `inmueble`
--
ALTER TABLE `inmueble`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=54;

--
-- AUTO_INCREMENT de la tabla `inquilino`
--
ALTER TABLE `inquilino`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=22;

--
-- AUTO_INCREMENT de la tabla `pago`
--
ALTER TABLE `pago`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=37;

--
-- AUTO_INCREMENT de la tabla `propietario`
--
ALTER TABLE `propietario`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=10;

--
-- AUTO_INCREMENT de la tabla `tipoinmueble`
--
ALTER TABLE `tipoinmueble`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=27;

--
-- AUTO_INCREMENT de la tabla `usuario`
--
ALTER TABLE `usuario`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=16;

--
-- Restricciones para tablas volcadas
--

--
-- Filtros para la tabla `contrato`
--
ALTER TABLE `contrato`
  ADD CONSTRAINT `fk_contrato_inmueble` FOREIGN KEY (`InmuebleId`) REFERENCES `inmueble` (`Id`) ON UPDATE CASCADE,
  ADD CONSTRAINT `fk_contrato_inquilino` FOREIGN KEY (`InquilinoId`) REFERENCES `inquilino` (`Id`) ON UPDATE CASCADE;

--
-- Filtros para la tabla `inmueble`
--
ALTER TABLE `inmueble`
  ADD CONSTRAINT `FK_Inmueble_TipoInmueble` FOREIGN KEY (`TipoId`) REFERENCES `tipoinmueble` (`Id`),
  ADD CONSTRAINT `fk_inmueble_prop` FOREIGN KEY (`PropietarioId`) REFERENCES `propietario` (`Id`) ON UPDATE CASCADE;

--
-- Filtros para la tabla `pago`
--
ALTER TABLE `pago`
  ADD CONSTRAINT `fk_pago_contrato` FOREIGN KEY (`ContratoId`) REFERENCES `contrato` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
