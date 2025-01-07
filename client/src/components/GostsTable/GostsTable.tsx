import {EditRounded, ImportContactsRounded} from "@mui/icons-material";
import {useAppSelector} from "../../app/hooks.ts";
import type {gostModel} from "../../entities/gost";
import type {GostSearchParams} from "../../entities/gost/gostModel.ts";
import {GenericTable} from "../GenericTable/GenericTable.tsx";
import GenericTableActionBlock from "../GenericTable/GenericTableActionBlock.tsx";
import GenericTableButton from "../GenericTable/GenericTableButton.tsx";

interface GostsTableProps {
	gosts: gostModel.GostViewInfo[];
	gostsParams: (GostSearchParams & { text?: string }) | null;
}

const GostsTable = ({ gosts, gostsParams }: GostsTableProps) => {
	const user = useAppSelector((s) => s.user.user);
	const columns = [
		{
			header: "Код ОКС",
			accessor: (g: gostModel.GostViewInfo) => g.codeOks,
		},
		{
			header: "Обозначение",
			accessor: (g: gostModel.GostViewInfo) => g.designation,
		},
		{
			header: "Наименование",
			accessor: (g: gostModel.GostViewInfo) => g.fullName,
		},
		gostsParams && Object.values(gostsParams).some((param) => param)
			? {
					header: "Соответствие запросу",
					accessor: (g: gostModel.GostViewInfo) =>
						gostsParams && Object.values(gostsParams).some((param) => param) ? g.relevanceMark : null,
				}
			: null,
		{
			header: "Действия",
			accessor: (g: gostModel.GostViewInfo) => (
				<GenericTableActionBlock>
					<GenericTableButton to={`/gost-review/${g.id}`}>
						<ImportContactsRounded />
					</GenericTableButton>
					{(user?.role === "Admin" || user?.role === "Heisenberg") && (
						<GenericTableButton to={`/gost-edit/${g.id}`}>
							<EditRounded />
						</GenericTableButton>
					)}
				</GenericTableActionBlock>
			),
		},
	].filter((column) => column !== null);

	return <GenericTable columns={columns} data={gosts} rowKey={"id"} />;
};

export default GostsTable;