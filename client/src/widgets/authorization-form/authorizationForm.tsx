import type React from "react";
import { useState } from "react";
import { Button, Input } from "../../shared/components";

import type { AxiosError } from "axios";
import { useFormik } from "formik";
import styles from "./AuthorizationForm.module.scss";
import type { UserAuthorization } from "./authorizationModel.ts";

interface AuthorizationFormProps {
	changeForm: Function;
	onSubmit: (user: UserAuthorization) => Promise<null | AxiosError>;
}

const AuthorizationForm: React.FC<AuthorizationFormProps> = (props) => {
	const { changeForm, onSubmit } = props;
	const [error, setError] = useState("");

	const validate = (values: { login: string; password: string }) => {
		const errors: { login?: string; password?: string } = {};

		if (!values.login) {
			errors.login = "Заполните поле";
		} /* else if (!/^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}$/i.test(values.login)) {
      errors.login = 'Некорректный логин';
    } */

		if (values.password.length < 7) {
			errors.password = "Пароль должен быть не меньше 7 символов";
		}

		return errors;
	};

	const formik = useFormik({
		initialValues: {
			login: "",
			password: "",
		},
		validate,
		onSubmit: (values) => {
			onSubmit(values).catch((err: AxiosError) => {
				if (err.response?.status === 400) {
					setError("Неправильный логин или пароль");
				}
			});
		},
	});

	return (
		<form
			className={styles.form}
			onSubmit={(event) => {
				event.preventDefault();
				formik.handleSubmit();
			}}
		>
			<Input
				type="text"
				id="login"
				name="login"
				label="Логин"
				placeholder="user@example.com"
				onChange={formik.handleChange("login")}
				value={formik.values.login}
				error={formik.errors.login}
			/>
			<Input
				type="password"
				label="Пароль"
				id="password"
				name="password"
				placeholder=""
				value={formik.values.password}
				onChange={formik.handleChange("password")}
				error={formik.errors.password}
			/>
			<div className={styles.buttonsContainer}>
				<Button isFilled className={styles.formButton} type="submit">
					Войти
				</Button>
				<Button className={styles.formButton} onClick={() => changeForm()} isColoredText>
					Зарегистрироваться
				</Button>
			</div>
			{error && <p className={styles.error}>{error}</p>}
		</form>
	);
};

export default AuthorizationForm;