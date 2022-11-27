-- phpMyAdmin SQL Dump
-- version 4.6.6
-- https://www.phpmyadmin.net/
--
-- Host: localhost
-- Creato il: Nov 22, 2021 alle 16:45
-- Versione del server: 5.7.17-log
-- Versione PHP: 5.6.30

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `mmorpgmaker`
--

-- --------------------------------------------------------

--
-- Struttura della tabella `chars`
--

CREATE TABLE `chars` (
  `char_id` int(10) NOT NULL,
  `account_id` int(10) NOT NULL,
  `char_num` int(10) NOT NULL,
  `name` varchar(255) NOT NULL,
  `class` varchar(255) NOT NULL,
  `base_level` int(10) NOT NULL,
  `job_level` int(10) NOT NULL,
  `base_exp` int(10) NOT NULL,
  `job_exp` int(10) NOT NULL,
  `gold` int(10) NOT NULL,
  `str` int(10) NOT NULL,
  `agi` int(10) NOT NULL,
  `vit` int(10) NOT NULL,
  `inte` int(10) NOT NULL,
  `dex` int(10) NOT NULL,
  `luk` int(10) NOT NULL,
  `max_hp` int(10) NOT NULL,
  `hp` int(10) NOT NULL,
  `max_sp` int(10) NOT NULL,
  `sp` int(10) NOT NULL,
  `status_point` int(10) NOT NULL,
  `skill_point` int(10) NOT NULL,
  `options` int(10) NOT NULL,
  `karma` int(10) NOT NULL,
  `party_id` int(10) NOT NULL,
  `guild_id` int(10) NOT NULL,
  `pet_id` int(10) NOT NULL,
  `body` int(10) NOT NULL,
  `weapon` int(10) NOT NULL,
  `shield` int(10) NOT NULL,
  `head` int(10) NOT NULL,
  `robe` int(10) NOT NULL,
  `accessory1` int(10) NOT NULL,
  `accessory2` int(10) NOT NULL,
  `last_map` varchar(255) NOT NULL,
  `last_x` int(10) NOT NULL,
  `last_y` int(10) NOT NULL,
  `save_map` varchar(255) NOT NULL,
  `save_x` int(10) NOT NULL,
  `save_y` int(10) NOT NULL,
  `partner_id` int(10) NOT NULL,
  `online` int(10) NOT NULL,
  `unban_time` int(10) NOT NULL,
  `sex` varchar(255) NOT NULL,
  `clan_id` int(10) NOT NULL,
  `show_equip` int(10) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Indici per le tabelle scaricate
--

--
-- Indici per le tabelle `chars`
--
ALTER TABLE `chars`
  ADD PRIMARY KEY (`char_id`);

--
-- AUTO_INCREMENT per le tabelle scaricate
--

--
-- AUTO_INCREMENT per la tabella `chars`
--
ALTER TABLE `chars`
  MODIFY `char_id` int(10) NOT NULL AUTO_INCREMENT;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
