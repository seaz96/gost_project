import type React from "react";
import { useContext, useState } from "react";

import { Link } from "react-router-dom";
import { Button } from "../../shared/components";
import styles from "./Header.module.scss";

import classNames from "classnames";
import { useAppDispatch } from "../../app/hooks.ts";
import { UserContext } from "../../entities/user";
import {logoutUser} from "../../features/user/userSlice.ts";
import account from "./assets/account.png";
import addIcon from "./assets/add-document.svg";
import arrowDown from "./assets/arrow-down.svg";
import exit from "./assets/exit.png";
import lock from "./assets/lock.png";
import profileIcon from "./assets/profile-icon.svg";

const ProfileDropdown = () => {
	const dispatch = useAppDispatch();

	return (
		<div className={styles.dropdown}>
			<Link to="/reset-password" style={{ color: "inherit" }}>
				<img src={lock} className={styles.dropdownImage} alt="reset password" />
				Сменить пароль
			</Link>
			<Link to={`/self-edit-page`} style={{ color: "inherit" }}>
				<img src={account} className={styles.dropdownImage} alt="edit profile" />
				Редактировать профиль
			</Link>
			<button
				type={"button"}
				className={styles.exitButton}
				onClick={() => {
					dispatch(logoutUser());
				}}
			>
				<img src={exit} className={styles.dropdownImage} alt="exit" />
				Выйти
			</button>
		</div>
	);
};

const Header = () => {
	const { user } = useContext(UserContext);
	const [isDropdownVisible, setDropdownVisible] = useState(false);

	const dropdownCloseHandler = () => {
		setDropdownVisible(false);
		document.removeEventListener("click", dropdownCloseHandler);
	};

	return (
		<header className={classNames(styles.header, "container")}>
			<div className={styles.buttonsContainer}>
				<Link to="/" className={classNames("baseButton", styles.headerButton)}>
					ВСЕ ДОКУМЕНТЫ
				</Link>
				{(user?.role === "Admin" || user?.role === "Heisenberg") && (
					<Button
						onClick={() => {}}
						isColoredText
						className={styles.headerButton}
						prefix={<img src={addIcon} alt="add" />}
					>
						<Link to="/gost-editor" style={{ color: "inherit" }}>
							СОЗДАТЬ ДОКУМЕНТ
						</Link>
					</Button>
				)}
			</div>
			<nav className={styles.headerNav}>
				<ul className={styles.headerNavList}>
					{(user?.role === "Admin" || user?.role === "Heisenberg") && (
						<li className={styles.navItem}>
							<Link to="/users-page" style={{ color: "inherit" }}>
								Пользователи
							</Link>
						</li>
					)}
					<li className={styles.navItem}>
						<Link to="/archive" style={{ color: "inherit" }}>
							Архив
						</Link>
					</li>
					<li className={styles.navItem}>
						<Link to="/statistic" style={{ color: "inherit" }}>
							Статистика
						</Link>
					</li>
				</ul>
			</nav>
			<div className={styles.profileContainer}>
				<Button
					onClick={(event: React.MouseEvent) => {
						event.stopPropagation();
						setDropdownVisible(!isDropdownVisible);
						document.addEventListener("click", dropdownCloseHandler);
					}}
					isColoredText
					className={styles.profileButton}
					prefix={<img src={profileIcon} alt="profile" style={{ display: "block", width: "22px", height: "22px" }} />}
					suffix={<img src={arrowDown} alt="open profile" style={{ display: "block", marginTop: "3px" }} />}
				>
					Профиль
				</Button>
				{isDropdownVisible && <ProfileDropdown />}
			</div>
		</header>
	);
};

export default Header;