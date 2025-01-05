import {useState} from "react";
import urfuLogo from "shared/assets/urfu.png";
import urfuLogoSvg from "shared/assets/urfu.svg";
import {useAppDispatch, useAppSelector} from "../../../app/hooks.ts";
import AuthorizationForm from "../../../components/AuthorizationForm/AuthorizationForm.tsx";
import type {UserAuthorization} from "../../../components/AuthorizationForm/authorizationModel.ts";
import RegistrationForm from "../../../components/RegistrationForm/RegistrationForm.tsx";
import type {UserRegistration} from "../../../components/RegistrationForm/registrationModel.ts";
import {loginUser} from "../../../features/user/userSlice.ts";
import styles from "./LoginPage.module.scss";

enum states {
	authorization = 0,
	registration = 1,
}

const LoginPage = () => {
	const [state, setState] = useState<states>(states.authorization);
	const dispatch = useAppDispatch();
	const error = useAppSelector((state) => state.user.error);

	const handleRegistration = async (user: UserRegistration) => {
		try {
			dispatch(loginUser(user)).unwrap();
		} catch (err) {
			console.error("Failed to register:", err);
		}
	};

	const handleAuthorization = async (user: UserAuthorization) => {
		try {
			dispatch(loginUser(user)).unwrap();
		} catch (err) {
			console.error("Failed to login:", err);
		}
	};

	return (
		<div className={styles.loginPageContainer}>
			<picture className={styles.logo}>
				<source srcSet={urfuLogoSvg} type="image/svg+xml" />
				<img src={urfuLogo} alt="logo" />
			</picture>
			{state === states.authorization ? (
				<section className={styles.authorizationForm}>
					<AuthorizationForm
						changeForm={() => setState(states.registration)}
						error={error}
						onSubmit={(user: UserAuthorization) => handleAuthorization(user)}
					/>
				</section>
			) : (
				<section className={styles.registrationForm}>
					<RegistrationForm
						changeForm={() => setState(states.authorization)}
						error={error}
						onSubmit={(user: UserRegistration) => handleRegistration(user)}
					/>
				</section>
			)}
		</div>
	);
};

export default LoginPage;