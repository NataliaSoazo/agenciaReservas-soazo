-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 06-09-2026 a las 02:55:53
-- Versión del servidor: 10.4.32-MariaDB
-- Versión de PHP: 8.0.30

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de datos: `agenciareservas`
--

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inmuebles`
--

CREATE TABLE `inmuebles` (
  `Id` int(11) NOT NULL,
  `Direccion` varchar(50) NOT NULL,
  `Uso` varchar(20) NOT NULL,
  `Tipo` varchar(20) NOT NULL,
  `Cupo` int(3) NOT NULL,
  `Precio` double(11,0) NOT NULL,
  `Latitud` varchar(20) NOT NULL,
  `Longitud` varchar(20) NOT NULL,
  `Disponible` varchar(4) NOT NULL,
  `Propietarioid` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `inmuebles`
--

INSERT INTO `inmuebles` (`Id`, `Direccion`, `Uso`, `Tipo`, `Cupo`, `Precio`, `Latitud`, `Longitud`, `Disponible`, `Propietarioid`) VALUES
(1, 'MAIPU 901', 'HABITACIONAL', 'LOCAL', 4, 325000, '1234567890', '0987654321', 'SI', 13),
(2, 'PEDERNERA 880', 'HABITACIONAL', 'LOCAL', 3, 125000, '1234567890', '0987654321', 'SI', 8),
(3, 'PEDERNERA 880', 'COMERCIAL', 'LOCAL', 3, 325000, '1234567890', '0987654321', 'SI', 15),
(4, 'SAN MARTIN 88', 'HABITACIONAL', 'DEPOSITO', 2, 325000, '1234567890', '0987654321', 'NO', 13),
(5, 'CHACO 34', 'HABITACIONAL', 'LOCAL', 2, 325000, '1234567890', '0987654321', 'SI', 13),
(6, 'CHACO 34', 'COMERCIAL', 'CASA', 3, 325000, '1234567890', '0987654321', 'SI', 13),
(8, 'BALCARSE 74', 'HABITACIONAL', 'CASA', 1, 150000, '133456776', '1234567788', 'SI', 22);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inquilinos`
--

CREATE TABLE `inquilinos` (
  `Id` int(11) NOT NULL,
  `Nombre` varchar(20) NOT NULL,
  `Apellido` varchar(20) NOT NULL,
  `Dni` varchar(12) NOT NULL,
  `Email` varchar(30) NOT NULL,
  `Telefono` varchar(20) NOT NULL,
  `Domicilio` varchar(20) NOT NULL,
  `Ciudad` varchar(20) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `inquilinos`
--

INSERT INTO `inquilinos` (`Id`, `Nombre`, `Apellido`, `Dni`, `Email`, `Telefono`, `Domicilio`, `Ciudad`) VALUES
(2, 'RAMON', 'LOPEZ', '123456654', 'LPRO@GMAIL.COM', '2663454325', 'LAS HERAS', 'MENDOZA'),
(3, 'MARTA', 'MOYANO', '17876543', 'MMOYANO@GMAIL.COM', '3516789677', 'AGUARIBAY 77', 'CÓRDOBA'),
(5, 'ALBERTO', 'MONTENEGRO', '46876377', 'RALBERTO@GMAIL.COM', '2664537777', 'TACUARI 80', 'SAN LUIS'),
(6, 'FABRICIO', 'ROMERO', '54872987', 'FROMERO@GMAIL.COM', '2776349987', 'SANTOS ORTIZ 99', 'SAN LUIS'),
(7, 'ALBERTO', 'MENDEZ', '99666123', 'ALBERTM@GMAIL.COM', '3447766880', 'LAS HERAS 987', 'SAN LUIS'),
(14, 'DAVID', 'GUTIERRES', '47798567', 'MARCELO.FERNANDEZ@MAIL.COM', '2664568798', 'LAS HERAS 98', 'SAN LUIS');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `propietarios`
--

CREATE TABLE `propietarios` (
  `Id` int(11) NOT NULL,
  `Nombre` varchar(20) NOT NULL,
  `Apellido` varchar(20) NOT NULL,
  `Dni` varchar(12) NOT NULL,
  `Email` varchar(30) NOT NULL,
  `Telefono` varchar(20) NOT NULL,
  `Domicilio` varchar(20) NOT NULL,
  `Ciudad` varchar(20) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `propietarios`
--

INSERT INTO `propietarios` (`Id`, `Nombre`, `Apellido`, `Dni`, `Email`, `Telefono`, `Domicilio`, `Ciudad`) VALUES
(1, 'NATALIA', 'SOAZO', '31542891', 'NATALIA.S.LABORAL@GMAIL.COM', '2664344567', 'FLORIDA 50', 'LA PUNTA'),
(3, 'ANAHI', 'CESPEDES', '24567765', 'ncespedes@gmail.com', '2664547689', 'Centenario 879', 'Sa'),
(7, 'CELESTE', 'AUMADA', '49288549', 'CAUMADA@GMAIL.COM', '1324365432', 'SAN MARTIN 80', 'SAN LUIS'),
(8, 'LORENZO', 'BENITEZ', '46876377', 'BLORENZ@GMAIL.COM', '2664536884', 'MITRE 223', 'SAN LUIS'),
(9, 'ALBERTO', 'SOSA', '44444444', 'GHDHHG@GMAIL.COM', '265435366', 'SANTA FE 877', 'SAN LUIS'),
(11, 'ANTONIO', 'CALDERÓN', '2563576536', 'ANTO@IMAIL', '5482314985', 'BELGRANO', 'SAN LUIS'),
(12, 'ALEJO', 'PEREZ', '2346519835', 'ALEJO@MAIL', '2664329754', 'SERRANA 76', 'SAN LUIS'),
(13, 'LISANDRO', 'AGUILER', '4563789210', 'AHGFKHLIA@MAIL', '2664873452', 'SAN MARTÍN 3405', 'SAN LUIS'),
(14, 'ABEL', 'CASTILLO', '1234554321', 'ABELITO@MAIL', '2664231768', 'MENDOZA', 'SAN LUIS'),
(15, 'TEODORO', 'BLANCO', '266754389', 'TEO@MAIL', '5367823145', 'CORDOBA', 'SAN LUIS'),
(17, 'PRUDENCIO', 'ANDRADE', '4256341724', 'JANDRADE@GMAIL.COM', '2554346578', '90', 'SAN LUIS'),
(18, 'JOAQUIN', 'VERA', '34652187', 'VJOAQUIN@GMAIL.COM', '2664536873', 'LAS VEGAS 18', 'SAN LUIS'),
(19, 'DANIEL', 'SOSA', '47787567', 'DSOSA@GMAIL.COM', '2665345768', 'MAIPU 19', 'SAN LUIS'),
(22, 'DAI', 'SAA', '47876567', '@GMAIL.COM', '26644547698', 'LAS HERAS 89', 'SAN LUIS'),
(23, 'ANDRES', 'VEGA', '3987678', 'VE@GMAIL.COM', '2664567890', 'LOS HALCONES 23', 'SAN LUIS'),
(24, 'ANDRES', 'VEGA', '678776567', 'SA@GMAIL.COM', '2665654567', 'MITRE  82', 'SAN LUIS');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `reservas`
--

CREATE TABLE `reservas` (
  `Id` int(11) NOT NULL,
  `Fecha` date NOT NULL,
  `FechaDesde` date NOT NULL,
  `FechaHasta` date NOT NULL,
  `Monto` double NOT NULL,
  `IdInquilino` int(11) NOT NULL,
  `IdInmueble` int(11) NOT NULL,
  `Anulado` tinyint(1) NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `reservas`
--

INSERT INTO `reservas` (`Id`, `Fecha`, `FechaDesde`, `FechaHasta`, `Monto`, `IdInquilino`, `IdInmueble`, `Anulado`) VALUES
(1, '2026-09-03', '2026-09-04', '2026-09-05', 130000, 7, 0, 0),
(2, '2026-09-04', '2026-09-11', '2026-09-12', 50000, 6, 0, 0),
(3, '2026-09-04', '2026-09-10', '2026-09-12', 55000.99, 14, 1, 0);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `tipoinmuebles`
--

CREATE TABLE `tipoinmuebles` (
  `Tipo` varchar(20) NOT NULL,
  `Id` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `tipoinmuebles`
--

INSERT INTO `tipoinmuebles` (`Tipo`, `Id`) VALUES
('LOCAL', 1),
('DEPOSITO', 2),
('CASA', 3),
('DEPARTAMENTO', 4);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `usoinmuebles`
--

CREATE TABLE `usoinmuebles` (
  `Uso` varchar(20) NOT NULL,
  `Id` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `usoinmuebles`
--

INSERT INTO `usoinmuebles` (`Uso`, `Id`) VALUES
('HABITACIONAL', 1),
('COMERCIAL', 2);

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `inmuebles`
--
ALTER TABLE `inmuebles`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `Propietarioid` (`Propietarioid`);

--
-- Indices de la tabla `inquilinos`
--
ALTER TABLE `inquilinos`
  ADD PRIMARY KEY (`Id`);

--
-- Indices de la tabla `propietarios`
--
ALTER TABLE `propietarios`
  ADD PRIMARY KEY (`Id`);

--
-- Indices de la tabla `reservas`
--
ALTER TABLE `reservas`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IdInmueble` (`IdInmueble`),
  ADD KEY `IdInquilino` (`IdInquilino`);

--
-- Indices de la tabla `tipoinmuebles`
--
ALTER TABLE `tipoinmuebles`
  ADD PRIMARY KEY (`Id`);

--
-- Indices de la tabla `usoinmuebles`
--
ALTER TABLE `usoinmuebles`
  ADD PRIMARY KEY (`Id`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `inmuebles`
--
ALTER TABLE `inmuebles`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=9;

--
-- AUTO_INCREMENT de la tabla `inquilinos`
--
ALTER TABLE `inquilinos`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=15;

--
-- AUTO_INCREMENT de la tabla `propietarios`
--
ALTER TABLE `propietarios`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=25;

--
-- AUTO_INCREMENT de la tabla `reservas`
--
ALTER TABLE `reservas`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT de la tabla `tipoinmuebles`
--
ALTER TABLE `tipoinmuebles`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT de la tabla `usoinmuebles`
--
ALTER TABLE `usoinmuebles`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- Restricciones para tablas volcadas
--

--
-- Filtros para la tabla `inmuebles`
--
ALTER TABLE `inmuebles`
  ADD CONSTRAINT `inmuebles_ibfk_1` FOREIGN KEY (`Propietarioid`) REFERENCES `propietarios` (`Id`);

--
-- Filtros para la tabla `reservas`
--
ALTER TABLE `reservas`
  ADD CONSTRAINT `reservas_ibfk_1` FOREIGN KEY (`IdInquilino`) REFERENCES `inquilinos` (`Id`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
