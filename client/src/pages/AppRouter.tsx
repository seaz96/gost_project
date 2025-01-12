import { Navigate, Route, Routes } from "react-router-dom";
import Header from "../components/Header/Header";
import { useFetchUserQuery } from "../features/api/apiSlice.ts";
import { GostCreatorPage } from "./GostCreatorPage";
import { LoginPage } from "./LoginPage";
import { StatisticsPage } from "./StatisticsPage";
import { GostEditPage } from "./gost-edit-page";
import { GostReviewPage } from "./gost-review-page";
import { GostsPage } from "./gosts-page";
import { ResetPasswordPage } from "./reset-password-page";
import { SelfEditPage } from "./self-edit-page";
import { UserEditPage } from "./user-edit-page";
import { UsersPage } from "./users-page";

const AppRouter = () => {
	//TODO: add skip
	const { data: user, isLoading } = useFetchUserQuery();

	if (isLoading) return null;

	return (
		<>
			{user && <Header />}
			{user ? (
				<Routes>
					<Route path="/" element={<GostsPage />} />
					<Route path="/gost-review/:id" element={<GostReviewPage />} />
					<Route path="/new" element={<GostCreatorPage />} />
					<Route path="/gost-edit/:id" element={<GostEditPage />} />
					<Route path="/users-page" element={<UsersPage />} />
					<Route path="/user-edit-page/:id" element={<UserEditPage />} />
					<Route path="/reset-password" element={<ResetPasswordPage />} />
					<Route path="/statistic" element={<StatisticsPage />} />
					<Route path="/self-edit-page" element={<SelfEditPage />} />
					<Route path="*" element={<Navigate to="/" replace />} />
				</Routes>
			) : (
				<Routes>
					<Route path="/login" element={<LoginPage />} />
					<Route path="*" element={<Navigate to="/login" replace />} />
				</Routes>
			)}
		</>
	);
};

export default AppRouter;