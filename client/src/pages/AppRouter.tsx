import { lazy } from "react";
import { Navigate, Route, Routes } from "react-router-dom";
import Header from "../components/Header/Header";
import { useFetchUserQuery } from "../features/api/apiSlice.ts";

const LoginPage = lazy(() => import("./LoginPage").then((module) => ({ default: module.LoginPage })));
const GostsPage = lazy(() => import("./gosts-page").then((module) => ({ default: module.GostsPage })));
const GostReviewPage = lazy(() => import("./gost-review-page").then((module) => ({ default: module.GostReviewPage })));
const GostEditorPage = lazy(() => import("./GostCreatorPage").then((module) => ({ default: module.GostCreatorPage })));
const UsersPage = lazy(() => import("./users-page").then((module) => ({ default: module.UsersPage })));
const ResetPasswordPage = lazy(() =>
	import("./reset-password-page").then((module) => ({ default: module.ResetPasswordPage })),
);
const GostEditPage = lazy(() => import("./gost-edit-page").then((module) => ({ default: module.GostEditPage })));
const UserEditPage = lazy(() => import("./user-edit-page").then((module) => ({ default: module.UserEditPage })));
const ArchivePage = lazy(() => import("./archive-page").then((module) => ({ default: module.ArchivePage })));
const StatisticPage = lazy(() => import("./statistic-page").then((module) => ({ default: module.StatisticPage })));
const ReviewsStatisticPage = lazy(() =>
	import("./reviews-statistic-page").then((module) => ({ default: module.ReviewsStatisticPage })),
);
const ChangesStatisticPage = lazy(() =>
	import("./changes-statistic-page").then((module) => ({ default: module.ChangesStatisticPage })),
);
const GostActualizePage = lazy(() =>
	import("./gost-actualize-page").then((module) => ({ default: module.GostActualizePage })),
);
const GostReplacePage = lazy(() =>
	import("./gost-replace-page").then((module) => ({ default: module.GostReplacePage })),
);
const SelfEditPage = lazy(() => import("./self-edit-page").then((module) => ({ default: module.SelfEditPage })));

const AppRouter = () => {
	const token = localStorage.getItem("jwt_token");

	const { data: user, isLoading } = useFetchUserQuery(undefined, {
		skip: !token,
	});

	if (isLoading) return null;

	return (
		<>
			{user && <Header />}
			{user ? (
				<Routes>
					<Route path="/" element={<GostsPage />} />
					<Route path="/gost-review/:id" element={<GostReviewPage />} />
					<Route path="/gost-editor" element={<GostEditorPage />} />
					<Route path="/gost-edit/:id" element={<GostEditPage />} />
					<Route path="/users-page" element={<UsersPage />} />
					<Route path="/user-edit-page/:id" element={<UserEditPage />} />
					<Route path="/reset-password" element={<ResetPasswordPage />} />
					<Route path="/archive" element={<ArchivePage />} />
					<Route path="/statistic" element={<StatisticPage />} />
					<Route path="/reviews-statistic-page" element={<ReviewsStatisticPage />} />
					<Route path="/changes-statistic-page" element={<ChangesStatisticPage />} />
					<Route path="/gost-actualize-page/:id" element={<GostActualizePage />} />
					<Route path="/gost-replace-page/:id" element={<GostReplacePage />} />
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