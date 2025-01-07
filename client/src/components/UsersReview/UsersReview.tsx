import {EditRounded} from "@mui/icons-material";
import { useAppSelector } from "../../app/hooks.ts";
import type { userModel } from "../../entities/user";
import { GenericTable } from "../GenericTable/GenericTable";
import GenericTableActionBlock from "../GenericTable/GenericTableActionBlock.tsx";
import GenericTableButton from "../GenericTable/GenericTableButton.tsx";

interface UsersTableProps {
	users: userModel.User[];
}

enum roles {
	User = "Пользователь",
	Admin = "Администратор",
	Heisenberg = "Главный администратор",
}

const UsersReview: React.FC<UsersTableProps> = ({ users }) => {
	const user = useAppSelector((state) => state.user.user);

	const columns = [
		{ header: "ID", accessor: (row: userModel.User) => row.id },
		{ header: "Роль", accessor: (row: userModel.User) => roles[row.role] },
		{ header: "Логин", accessor: (row: userModel.User) => row.login },
		{ header: "Фио", accessor: (row: userModel.User) => row.name },
		{
			header: "Действия",
			accessor: (row: userModel.User) =>
				row.id !== 0 && (user?.role === "Admin" || user?.role === "Heisenberg") ? (
					<GenericTableActionBlock>
						<GenericTableButton to={`/user-edit-page/${row.id}`}>
							<EditRounded />
						</GenericTableButton>
					</GenericTableActionBlock>
				) : null,
		}
	];

	return <GenericTable columns={columns} data={users} rowKey="id" />;
};

export default UsersReview;