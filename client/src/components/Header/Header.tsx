import classNames from "classnames";
import { type ReactNode, useState } from "react";
import { Link } from "react-router-dom";
import { useAppDispatch, useAppSelector } from "../../app/hooks.ts";
import { logoutUser } from "../../features/user/userSlice.ts";
import urfuLogo from "../../shared/assets/urfu.png";
import urfuLogoSvg from "../../shared/assets/urfu.svg";
import urfuProfile from "../../shared/assets/urfuProfile.svg";
import styles from "./Header.module.scss";

const ProfileDropdown = () => {
	const dispatch = useAppDispatch();

	return (
		<div className={styles.dropdown}>
			<Link to="/reset-password">Сменить пароль</Link>
			<Link to={"/self-edit-page"}>Редактировать профиль</Link>
			<button
				type={"button"}
				onClick={() => {
					dispatch(logoutUser());
				}}
			>
				Выйти
			</button>
		</div>
	);
};

const HeaderLink = ({ to, children }: { to: string; children: ReactNode }) => {
	return (
		<li>
			<Link to={to} className={styles.headerLink}>
				{children}
			</Link>
		</li>
	);
};

const HeaderProfileIcon = () => {
	return (
		<div className={styles.profileIcon}>
			<img src={urfuProfile} alt="profile" />
		</div>
	);
};

const Header = () => {
	const user = useAppSelector((s) => s.user.user);
	const [isDropdownVisible, setDropdownVisible] = useState(false);

	const dropdownCloseHandler = () => {
		setDropdownVisible(false);
		document.removeEventListener("click", dropdownCloseHandler);
	};

	return (
		<header className={classNames(styles.header, "container")}>
			<Link to={"/"}>
				<picture className={styles.logo}>
					<source srcSet={urfuLogoSvg} type="image/svg+xml" />
					<img src={urfuLogo} alt="logo" />
				</picture>
			</Link>
			<nav className={styles.buttonsContainer}>
				<ul>
					<HeaderLink to="/">Документы</HeaderLink>
					<HeaderLink to="/statistic">Статистика</HeaderLink>
					{(user?.role === "Admin" || user?.role === "Heisenberg") && (
						<>
							<HeaderLink to={"/gost-editor"}>Создать документ</HeaderLink>
							<HeaderLink to="/users-page">Пользователи</HeaderLink>
						</>
					)}
				</ul>
			</nav>
			<div className={styles.profileContainer}>
				<button
					onClick={(event) => {
						event.stopPropagation();
						setDropdownVisible(!isDropdownVisible);
						document.addEventListener("click", dropdownCloseHandler);
					}}
					type={"button"}
				>
					<HeaderProfileIcon />
				</button>
				{isDropdownVisible && <ProfileDropdown />}
			</div>
		</header>
	);
};

export default Header;