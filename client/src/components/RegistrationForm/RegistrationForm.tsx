import type React from "react";
import { useForm } from "react-hook-form";
import UrfuButton from "../../shared/components/Button/UrfuButton.tsx";
import UrfuTextInput from "../../shared/components/Input/UrfuTextInput.tsx";
import styles from "./RegistrationForm.module.scss";
import type { UserRegistration } from "./registrationModel.ts";

interface RegistrationFormProps {
	changeForm: () => void;
	onSubmit: (user: UserRegistration) => Promise<void>;
	error: string | null;
}

type FormValues = UserRegistration & { repeatedPassword: string };

const RegistrationForm: React.FC<RegistrationFormProps> = (props) => {
	const { changeForm, onSubmit, error } = props;

	const {
		register,
		handleSubmit,
		formState: { errors },
		watch,
	} = useForm<FormValues>({
		defaultValues: {
			login: "",
			password: "",
			repeatedPassword: "",
			role: "",
			name: "",
			orgName: "",
			orgBranch: "",
			orgActivity: "",
		},
	});

	const password = watch("password");

	const onSubmitHandler = (data: FormValues) => {
		onSubmit({ ...data, role: "Admin" });
	};

	return (
		<form className={styles.form} onSubmit={handleSubmit(onSubmitHandler)}>
			<UrfuTextInput
				type="email"
				label="Логин"
				placeholder="user@example.com"
				{...register("login", {
					required: "Заполните поле",
					pattern: {
						value: /^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}$/i,
						message: "Некорректный логин",
					},
				})}
				error={errors.login?.message}
			/>
			<UrfuTextInput
				type="password"
				label="Пароль"
				{...register("password", {
					required: "Заполните поле",
					minLength: {
						value: 7,
						message: "Пароль должен быть не меньше 7 символов",
					},
				})}
				error={errors.password?.message}
			/>
			<UrfuTextInput
				type="password"
				label="Повторите пароль"
				{...register("repeatedPassword", {
					required: "Заполните поле",
					validate: (value) => value === password || "Пароли не совпадают",
				})}
				error={errors.repeatedPassword?.message}
			/>
			<UrfuTextInput
				type="text"
				label="ФИО пользователя"
				placeholder="Иванов Иван Иванович"
				{...register("name", {
					required: "Заполните поле",
				})}
				error={errors.name?.message}
			/>
			<UrfuTextInput
				type="text"
				label="Название организации"
				placeholder="Уральский федеральный университет"
				{...register("orgName", {
					required: "Заполните поле",
				})}
				error={errors.orgName?.message}
			/>
			<UrfuTextInput
				type="text"
				label="Отделение организации"
				placeholder="Институт радиоэлектроники и информационных технологий - РтФ"
				{...register("orgBranch", {
					required: "Заполните поле",
				})}
				error={errors.orgBranch?.message}
			/>
			<UrfuTextInput
				type="text"
				label="Деятельность организации"
				placeholder="Образование"
				{...register("orgActivity", {
					required: "Заполните поле",
				})}
				error={errors.orgActivity?.message}
			/>
			{error && <p className={styles.error}>{error}</p>}
			<div className={styles.buttonsContainer}>
				<UrfuButton type="submit">Зарегистрироваться</UrfuButton>
				<UrfuButton onClick={() => changeForm()} outline={true}>
					Авторизоваться
				</UrfuButton>
			</div>
		</form>
	);
};

export default RegistrationForm;
