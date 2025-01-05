import { useContext, useState } from "react";

import { UserContext, type userModel } from "entities/user";
import urfuLogo from "shared/assets/urfu.png";
import { axiosInstance } from "shared/configs/axiosConfig";
import AuthorizationForm from "../../../widgets/authorization-form/authorizationForm.tsx";
import type {UserAuthorization} from "../../../widgets/authorization-form/authorizationModel.ts";
import RegistrationForm from "../../../widgets/registration-form/RegistrationForm.tsx";
import type {UserRegistration} from "../../../widgets/registration-form/registrationModel.ts";
import styles from "./LoginPage.module.scss";

enum states {
	authorization = 0,
	registration = 1,
}

const LoginPage = () => {
	const [state, setState] = useState<states>(states.authorization);
	const { setUser } = useContext(UserContext);

	const handleRegistration = async (user: UserRegistration) => {
		axiosInstance
			.post<userModel.User>("/accounts/register", user)
			.then((response) => {
				setUser(response.data);
				localStorage.setItem("jwt_token", response.data.token);
				window.location.href = "/";
			})
			.catch((error) => console.log(error));
	};

	const handleAuthorization = (user: UserAuthorization) => {
		return axiosInstance.post<userModel.User>("/accounts/login", user).then((response) => {
			setUser(response.data);
			localStorage.setItem("jwt_token", response.data.token);
			window.location.href = "/";
			return null;
		});
	};

	return (
		<div className={styles.loginPageContainer}>
			<img className={styles.logo} src={urfuLogo} alt="logo" />
			{state === states.authorization ? (
				<section className={styles.authorizationForm}>
					<AuthorizationForm
						changeForm={() => setState(states.registration)}
						onSubmit={(user: UserAuthorization) => handleAuthorization(user)}
					/>
				</section>
			) : (
				<section className={styles.registrationForm}>
					<RegistrationForm
						changeForm={() => setState(states.authorization)}
						onSubmit={(user: UserRegistration) => handleRegistration(user)}
					/>
				</section>
			)}
		</div>
	);
};

export default LoginPage;