import type { gostModel } from "../../entities/gost";
import { GenericTable } from "../GenericTable/GenericTable.tsx";

interface ReviewsStatisticTableProps {
	reviewsData: gostModel.GostViews[];
}

const ReviewsStatisticTable: React.FC<ReviewsStatisticTableProps> = (props) => {
	const { reviewsData } = props;

	const columns = [
		{ header: "ID", accessor: (g: gostModel.GostViews) => g.docId },
		{ header: "Обозначение", accessor: (g: gostModel.GostViews) => g.designation },
		{ header: "Сфера деятельности", accessor: (g: gostModel.GostViews) => g.fullName },
		{ header: "Просмотры", accessor: (g: gostModel.GostViews) => g.views },
	];

	return <GenericTable columns={columns} data={reviewsData} rowKey={"docId"} />;
};

export default ReviewsStatisticTable;
