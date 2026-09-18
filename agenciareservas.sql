-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 18-09-2026 a las 04:09:09
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
  `Precio` decimal(11,0) NOT NULL,
  `Latitud` varchar(20) NOT NULL,
  `Longitud` varchar(20) NOT NULL,
  `Disponible` varchar(4) NOT NULL,
  `Propietarioid` int(11) NOT NULL,
  `porcentual` int(3) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `inmuebles`
--

INSERT INTO `inmuebles` (`Id`, `Direccion`, `Uso`, `Tipo`, `Cupo`, `Precio`, `Latitud`, `Longitud`, `Disponible`, `Propietarioid`, `porcentual`) VALUES
(1, 'MAIPU 901', 'HABITACIONAL', 'LOCAL', 4, 325000, '1234567890', '0987654321', 'SI', 13, 20),
(2, 'PEDERNERA 880', 'HABITACIONAL', 'LOCAL', 3, 125000, '1234567890', '0987654321', 'SI', 8, 20),
(3, 'PEDERNERA 880', 'COMERCIAL', 'LOCAL', 3, 325000, '1234567890', '0987654321', 'SI', 15, 20),
(4, 'SAN MARTIN 88', 'HABITACIONAL', 'DEPOSITO', 2, 325000, '1234567890', '0987654321', 'NO', 13, 20),
(5, 'CHACO 34', 'HABITACIONAL', 'LOCAL', 2, 325000, '1234567890', '0987654321', 'SI', 13, 20),
(6, 'CHACO 34', 'COMERCIAL', 'CASA', 3, 325000, '1234567890', '0987654321', 'SI', 13, 20),
(9, 'CASEROS 210', 'HABITACIONAL', 'CASA', 2, 50000, '1234567889', '123456789', 'SI', 15, 20),
(10, 'MITRE 80', 'HABITACIONAL', 'CASA', 2, 50000, '1234567890', '0987654321', 'SI', 7, 20),
(11, 'PEDERNERA 1201', 'HABITACIONAL', 'DEPARTAMENTO', 1, 50000, '1234567890', '0987654321', 'SI', 13, 20),
(12, 'LAFINUR 230', 'HABITACIONAL', 'CASA', 2, 50000, '1234567890', '0987654321', 'SI', 13, 20),
(13, 'CHACO 06', 'COMERCIAL', 'CASA', 2, 325000, '1234567890', '0987654321', 'SI', 14, 20);

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
(3, 'MARTA', 'MOYANO', '17876543', 'MMOYANO@GMAIL.COM', '3516789677', 'AGUARIBAY 77', 'CÓRDOBA'),
(5, 'ALBERTO', 'MONTENEGRO', '46876377', 'RALBERTO@GMAIL.COM', '2664537777', 'TACUARI 80', 'SAN LUIS'),
(6, 'FABRICIO', 'ROMERO', '54872987', 'FROMERO@GMAIL.COM', '2776349987', 'SANTOS ORTIZ 99', 'SAN LUIS'),
(7, 'ALBERTO', 'MENDEZ', '99666123', 'ALBERTM@GMAIL.COM', '3447766880', 'LAS HERAS 987', 'SAN LUIS'),
(14, 'DAVID', 'GUTIERREZ', '47798567', 'MARCELO.FERNANDEZ@MAIL.COM', '2664568798', 'LAS HERAS 98', 'SAN LUIS'),
(16, 'XXXXXX', 'XXXXX', '5487298', 'XXXX@GMAIL.COM', '3446544321', 'OLASCOAGA 18', 'MENDOZA'),
(18, 'JOAQUIN', 'VERA', '34652187', 'VJOAQUIN@GMAIL.COM', '2664536873', 'LAS VEGAS 18', 'SAN LUIS'),
(19, 'DANIEL', 'SOSA', '47787567', 'DSOSA@GMAIL.COM', '2665345768', 'MAIPU 19', 'SAN LUIS'),
(21, 'NATALIA', 'SOAZO', '31542891', 'NATALIA.S.LABORAL@GMAIL.COM', '2664344567', 'FLORIDA 50', 'LA PUNTA'),
(23, 'ANAHI', 'CESPEDES', '24567765', 'ncespedes@gmail.com', '2664547689', 'Centenario 879', 'Sa'),
(27, 'CELESTE', 'AUMADA', '49288549', 'CAUMADA@GMAIL.COM', '1324365432', 'SAN MARTIN 80', 'SAN LUIS'),
(28, 'LORENZO', 'BENITEZ', '46876387', 'BLORENZ@GMAIL.COM', '2664536884', 'MITRE 223', 'SAN LUIS'),
(29, 'ALBERTO', 'SOSA', '44444444', 'GHDHHG@GMAIL.COM', '265435366', 'SANTA FE 877', 'SAN LUIS'),
(32, 'ALEJO', 'PEREZ', '2346519835', 'ALEJO@MAIL', '2664329754', 'SERRANA 76', 'SAN LUIS'),
(33, 'LISANDRO', 'AGUILER', '45637892', 'AHGFKHLIA@MAIL', '2664873452', 'SAN MARTÍN 3405', 'SAN LUIS'),
(34, 'ANDRES', 'VEGA', '678776567', 'SA@GMAIL.COM', '2665654567', 'MITRE  82', 'SAN LUIS'),
(35, 'TEODORO', 'BLANCO', '26675438', 'TEO@MAIL', '5367823145', 'CORDOBA', 'SAN LUIS'),
(37, 'PRUDENCIO', 'ANDRADE', '42563417', 'JANDRADE@GMAIL.COM', '2554346578', 'LAVALLE 90', 'SAN LUIS'),
(42, 'DAI', 'SAA', '47876567', '@GMAIL.COM', '26644547698', 'LAS HERAS 89', 'SAN LUIS'),
(53, 'ANDRES', 'VEGA', '3987678', 'VE@GMAIL.COM', '2664567890', 'LOS HALCONES 23', 'SAN LUIS'),
(54, 'ABEL', 'CASTILLO', '12345543', 'ABELITO@MAIL', '2664231768', 'MENDOZA', 'SAN LUIS'),
(411, 'ANTONIO', 'CALDERÓN', '25635765', 'ANTO@IMAIL', '5482314985', 'BELGRANO', 'SAN LUIS');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `pagos`
--

CREATE TABLE `pagos` (
  `Id` int(11) NOT NULL,
  `Fecha` date NOT NULL,
  `IdReserva` int(11) NOT NULL,
  `Modo` varchar(100) NOT NULL,
  `Concepto` varchar(255) NOT NULL,
  `Importe` decimal(10,2) NOT NULL,
  `Anulado` tinyint(1) NOT NULL DEFAULT 0,
  `IdAlta` int(11) NOT NULL,
  `IdBaja` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `pagos`
--

INSERT INTO `pagos` (`Id`, `Fecha`, `IdReserva`, `Modo`, `Concepto`, `Importe`, `Anulado`, `IdAlta`, `IdBaja`) VALUES
(1, '2026-09-17', 5, 'EFECTIVO', 'PAGO TOTAL', 1125000.00, 0, 1, 0),
(2, '2026-09-17', 6, 'TRANSFERENCIA', 'PAGO TOTAL', 2925000.00, 0, 3, 0),
(3, '2026-09-17', 3, 'TARJETA', 'PAGO TOTAL', 650000.00, 0, 3, 0),
(4, '2026-09-17', 7, 'TARJETA', 'PAGO TOTAL', 100000.00, 0, 3, 0);

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
(11, 'ANTONIO', 'CALDERÓN', '25635765', 'XXANTO@GIMAIL', '5482314985', 'BELGRANO', 'SAN LUIS'),
(12, 'ALEJO', 'PEREZ', '23465198', 'GGGALEJO@MAIL', '2664329754', 'SERRANA 76', 'SAN LUIS'),
(13, 'LISANDRO', 'AGUILAR', '45637892', 'AHGFKHLIA@MAIL', '2664873452', 'SAN MARTÍN 3405', 'SAN LUIS'),
(14, 'ABEL', 'CASTILLO', '12345543', 'CCABELITO@MAIL', '2664231768', 'MENDOZA', 'SAN LUIS'),
(15, 'TEODORO', 'BLANCO', '26675438', 'FFFFFTEO@MAIL', '5367823145', 'CORDOBA', 'SAN LUIS'),
(17, 'PRUDENCIO', 'ANDRADE', '42563417', 'JANDRADE@GMAIL.COM', '2554346578', 'FALUCHO 90', 'SAN LUIS'),
(18, 'JOAQUIN', 'VERA', '34652187', 'VJOAQUIN@GMAIL.COM', '2664536873', 'LAS VEGAS 18', 'SAN LUIS'),
(19, 'DANIEL', 'SOSA', '47787567', 'DSOSA@GMAIL.COM', '2665345768', 'MAIPU 19', 'SAN LUIS'),
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
  `Monto` decimal(10,0) NOT NULL,
  `IdInquilino` int(11) NOT NULL,
  `IdInmueble` int(11) NOT NULL,
  `Anulado` tinyint(1) NOT NULL DEFAULT 0,
  `IdBaja` int(11) DEFAULT NULL,
  `IdAlta` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `reservas`
--

INSERT INTO `reservas` (`Id`, `Fecha`, `FechaDesde`, `FechaHasta`, `Monto`, `IdInquilino`, `IdInmueble`, `Anulado`, `IdBaja`, `IdAlta`) VALUES
(1, '2026-09-03', '2026-09-04', '2026-09-05', 130000, 7, 0, 0, 0, 3),
(2, '2026-09-04', '2026-09-11', '2026-09-12', 50000, 6, 0, 0, 0, 3),
(3, '2026-09-04', '2026-09-10', '2026-09-12', 55001, 14, 1, 0, 0, 3),
(4, '2026-09-06', '2026-09-06', '2026-09-11', 350000, 14, 2, 1, 0, 3),
(5, '2026-09-16', '2026-09-16', '2026-09-25', 450000, 14, 2, 0, 0, 3),
(6, '2026-09-16', '2026-09-16', '2026-09-25', 150000, 7, 1, 0, 0, 3),
(7, '2026-09-17', '2026-09-18', '2026-09-20', 250000, 23, 10, 0, 0, 3);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `roles`
--

CREATE TABLE `roles` (
  `Numero` int(1) NOT NULL,
  `Rol` varchar(15) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `roles`
--

INSERT INTO `roles` (`Numero`, `Rol`) VALUES
(1, 'EMPLEADO'),
(2, 'ADMINISTRADOR');

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

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `usuarios`
--

CREATE TABLE `usuarios` (
  `Id` int(11) NOT NULL,
  `Nombre` varchar(20) NOT NULL,
  `Apellido` varchar(20) NOT NULL,
  `Correo` varchar(30) NOT NULL,
  `Clave` varchar(1500) NOT NULL,
  `Rol` int(2) NOT NULL,
  `AvatarURL` varchar(30) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `usuarios`
--

INSERT INTO `usuarios` (`Id`, `Nombre`, `Apellido`, `Correo`, `Clave`, `Rol`, `AvatarURL`) VALUES
(1, 'ADRIAN ', 'LOPEZ', 'adrian@gmail.com', 'acZaSPRv70HvJDWwjGs/+MowxfUra9L3T4oMkjSPG/E=', 1, '/ImgSubidas\\anonimo.jpg'),
(3, 'NATALIA', 'SOAZO', 'natalia.s.laboral@gmail.com', 'acZaSPRv70HvJDWwjGs/+MowxfUra9L3T4oMkjSPG/E=', 2, '/ImgSubidas\\2av_3.jpeg');

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
  ADD PRIMARY KEY (`Id`),
  ADD UNIQUE KEY `Dni` (`Dni`);

--
-- Indices de la tabla `pagos`
--
ALTER TABLE `pagos`
  ADD PRIMARY KEY (`Id`);

--
-- Indices de la tabla `propietarios`
--
ALTER TABLE `propietarios`
  ADD PRIMARY KEY (`Id`),
  ADD UNIQUE KEY `Dni` (`Dni`);

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
-- Indices de la tabla `usuarios`
--
ALTER TABLE `usuarios`
  ADD PRIMARY KEY (`Id`),
  ADD UNIQUE KEY `Correo` (`Correo`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `inmuebles`
--
ALTER TABLE `inmuebles`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=14;

--
-- AUTO_INCREMENT de la tabla `inquilinos`
--
ALTER TABLE `inquilinos`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=412;

--
-- AUTO_INCREMENT de la tabla `pagos`
--
ALTER TABLE `pagos`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT de la tabla `propietarios`
--
ALTER TABLE `propietarios`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=26;

--
-- AUTO_INCREMENT de la tabla `reservas`
--
ALTER TABLE `reservas`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;

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
-- AUTO_INCREMENT de la tabla `usuarios`
--
ALTER TABLE `usuarios`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

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
