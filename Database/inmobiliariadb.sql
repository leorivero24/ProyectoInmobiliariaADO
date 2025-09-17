-- phpMyAdmin SQL Dump
-- version 5.1.0
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 17-09-2025 a las 03:15:16
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
  `Observaciones` text DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Volcado de datos para la tabla `contrato`
--

INSERT INTO `contrato` (`Id`, `InmuebleId`, `InquilinoId`, `FechaInicio`, `FechaFin`, `Monto`, `Periodicidad`, `Estado`, `Observaciones`) VALUES
(7, 2, 14, '2024-07-29', '2025-07-29', '32000.00', 'Anual', 'Finalizado', 'Finalizo Julio 2025'),
(8, 2, 18, '2025-06-29', '2029-09-29', '32000.00', 'Mensual', 'Vigente', 'Sigue Vigente');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inmueble`
--

CREATE TABLE `inmueble` (
  `Id` int(11) NOT NULL,
  `Direccion` varchar(255) NOT NULL,
  `Tipo` varchar(50) DEFAULT NULL,
  `Ambientes` int(11) DEFAULT NULL,
  `Superficie` decimal(10,2) DEFAULT NULL,
  `Precio` decimal(12,2) DEFAULT NULL,
  `PropietarioId` int(11) NOT NULL,
  `Estado` varchar(50) DEFAULT 'Disponible',
  `Observaciones` text DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Volcado de datos para la tabla `inmueble`
--

INSERT INTO `inmueble` (`Id`, `Direccion`, `Tipo`, `Ambientes`, `Superficie`, `Precio`, `PropietarioId`, `Estado`, `Observaciones`) VALUES
(2, 'Sarmiento 2903', 'Monoambiente ', 1, '230.00', '32000.00', 1, 'Disponible', 'Ideal 2 personas, patio chico'),
(5, 'Sarmiento 2892', 'Duplex', 2, '320.00', '39000.00', 5, 'Disponible', 'Grande'),
(17, 'Calle Falsa 1238', 'Departamento', 3, '75.50', '120000.00', 1, 'Alquilado', 'Luminoso y céntrico'),
(19, 'Calle Libertad 45', 'Departamento', 2, '60.00', '95000.00', 6, 'Disponible', 'Recientemente remodelado'),
(20, 'Boulevard Central 100', 'Oficina', 4, '120.00', '180000.00', 7, 'Disponible', 'Ideal para consultorio'),
(28, 'Av. Libertador 1234', 'Departamento', 3, '85.50', '120000.00', 1, 'Disponible', 'Departamento con balcón y vista al río'),
(29, 'Calle Falsa 742', 'Casa', 4, '120.00', '150000.00', 5, 'Disponible', 'Casa familiar con jardín y garage'),
(30, 'Av. Corrientes 2000', 'Local', 1, '60.00', '80000.00', 6, 'Alquilado', 'Local comercial en esquina, alto tránsito'),
(31, 'Calle Córdoba 1500', 'Departamento', 2, '70.00', '95000.00', 7, 'Disponible', 'Departamento amoblado, ideal estudiantes'),
(34, 'Avenida Lafinur', 'Monoambiente', 1, '1200.00', '43000.00', 1, 'Reservado', 'Funcional y comodo');

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
(3, '12345678', 'Ana', 'López', '555-1234', 'ana.lopez@email.com', 'Calle Primavera 123'),
(14, '23456789', 'Luis', 'Martínez', '555-5678', 'luis.martinez@email.com', 'Avenida Central 456'),
(17, '34567890', 'María', 'Gómez', '555-9012', 'maria.gomez@email.com', 'Calle Sol 789'),
(18, '45678901', 'Carlos', 'Ramírez', '555-3456', 'carlos.ramirez@email.com', 'Boulevard Norte 321'),
(19, '56789012', 'Sofía', 'Fernández', '555-7890', 'sofia.fernandez@email.com', 'Calle Luna 654');

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
  `Observaciones` text DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

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
(1, '28789404', 'Leonardo', 'Rivero', '2664642996', 'leonardo.rivero@email.com', 'Calle Falsa 123'),
(5, '294424242', 'Cliff', ' Burton', '2664255432', 'cliff.burton@email.com', 'Avenida Siempre Viva 742'),
(6, '12345678', 'Juan', 'Pérez', '1234567890', 'juan.perez@email.com', 'Calle Roble 101'),
(7, '23456789', 'María', 'Gómez', '0987654321', 'maria.gomez@email.com', 'Avenida Olmos 202'),
(8, '34567890', 'Carlos', 'Ramírez', '1122334455', 'carlos.ramirez@email.com', 'Boulevard Pino 303');

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
  ADD KEY `fk_inmueble_prop` (`PropietarioId`);

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
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `contrato`
--
ALTER TABLE `contrato`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=9;

--
-- AUTO_INCREMENT de la tabla `inmueble`
--
ALTER TABLE `inmueble`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=37;

--
-- AUTO_INCREMENT de la tabla `inquilino`
--
ALTER TABLE `inquilino`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=20;

--
-- AUTO_INCREMENT de la tabla `pago`
--
ALTER TABLE `pago`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `propietario`
--
ALTER TABLE `propietario`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=9;

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
