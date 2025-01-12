import { useForm } from "react-hook-form";
import UrfuTextInput from "shared/components/Input/UrfuTextInput.tsx";
import { useAppSelector } from "../../app/hooks.ts";
import type { User } from "../../entities/user/userModel.ts";
import UrfuButton from "../../shared/components/Button/UrfuButton.tsx";
import UrfuCheckbox from "../../shared/components/Input/UrfuCheckbox.tsx";
import styles from "./UserEditForm.module.scss";
import type { UserEditType } from "./userEditModel.ts";

interface UserEditFormProps {
	handleSubmit: (userData: UserEditType) => void;
	userData: User;
}

const UserEditForm = (props: UserEditFormProps) => {
	const { handleSubmit, userData } = props;
	const user = useAppSelector((s) => s.user.user);

	const {
		register,
		handleSubmit: handleFormSubmit,
		watch,
		formState: { errors },
	} = useForm<UserEditType>({
		defaultValues: {
			name: userData.name,
			login: userData.login,
			org_name: userData.orgName,
			org_activity: userData.orgActivity,
			org_branch: userData.orgBranch,
			is_admin: userData.role === "Admin" || userData.role === "Heisenberg",
		},
	});

	return (
		<form className={styles.form} onSubmit={handleFormSubmit((data) => handleSubmit(data))}>
			<UrfuTextInput label="ID" disabled={true} type="text" value={userData.id} />
			<UrfuTextInput label="ФИО пользователя" type="text" {...register("name")} />
			<UrfuTextInput
				label="Логин"
				type="text"
				error={errors.login?.message}
				{...register("login", {
					required: "Логин обязателен для заполнения",
				})}
			/>
			<UrfuTextInput label="Название организации" type="text" {...register("org_name")} />
			<UrfuTextInput label="Отделение организации" type="text" {...register("org_branch")} />
			<UrfuTextInput label="Деятельность организации" type="text" {...register("org_activity")} />
			{userData.id !== user?.id && user?.role === "Heisenberg" && (
				<UrfuCheckbox
					title={"Пользователь является администратором"}
					{...register("is_admin")}
					checked={watch("is_admin")}
				/>
			)}
			<UrfuButton type="submit">Сохранить</UrfuButton>
		</form>
	);
};

export default UserEditForm;
