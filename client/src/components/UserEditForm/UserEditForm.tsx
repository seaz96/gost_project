import type React from "react";
import { useState } from "react";
import { useAppSelector } from "../../app/hooks.ts";
import type { User } from "../../entities/user/userModel.ts";
import { Button, Input } from "../../shared/components";
import styles from "./UserEditForm.module.scss";
import type { UserEditType } from "./userEditModel.ts";

interface UserEditFormProps {
	handleSubmit: Function;
	userData: User;
	id: number;
}

const UserEditForm: React.FC<UserEditFormProps> = (props) => {
	const { handleSubmit, userData, id } = props;
	const user = useAppSelector((s) => s.user.user);

	const [userEditData, setUserEditData] = useState<UserEditType>({
		name: userData.name,
		login: userData.login,
		org_name: userData.orgName,
		org_activity: userData.orgActivity,
		org_branch: userData.orgBranch,
		is_admin: userData.role === "Admin" || userData.role === "Heisenberg",
	});

	return (
		<form
			className={styles.form}
			onSubmit={(event) => {
				event.preventDefault();
				handleSubmit(userEditData);
			}}
		>
			<Input
				label="ФИО пользователя"
				type="text"
				value={userEditData.name}
				onChange={(value: string) => setUserEditData({ ...userEditData, name: value })}
			/>
			<Input
				label="Логин"
				type="text"
				value={userEditData.login}
				onChange={(value: string) => setUserEditData({ ...userEditData, login: value })}
			/>
			<Input
				label="Название организации"
				type="text"
				value={userEditData.org_name}
				onChange={(value: string) => setUserEditData({ ...userEditData, org_name: value })}
			/>
			<Input
				label="Отделение организации"
				type="text"
				value={userEditData.org_branch}
				onChange={(value: string) => setUserEditData({ ...userEditData, org_branch: value })}
			/>
			<Input
				label="Деятельность организации"
				type="text"
				value={userEditData.org_activity}
				onChange={(value: string) => setUserEditData({ ...userEditData, org_activity: value })}
			/>
			{id !== user?.id && user?.role === "Heisenberg" && (
				<div className={styles.checkboxContainer}>
					<input
						type="checkbox"
						id="switchAdmin"
						onChange={() =>
							setUserEditData({
								...userEditData,
								is_admin: !userEditData.is_admin,
							})
						}
						checked={userEditData.is_admin}
					/>
					<label htmlFor="switchAdmin">Пользователь является администратором</label>
				</div>
			)}
			<Button type="submit" onClick={() => {}} className={styles.formButton} isFilled>
				Сохранить
			</Button>
		</form>
	);
};

export default UserEditForm;
