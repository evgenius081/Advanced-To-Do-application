import { ItemPriority } from "../../enums/item-priority";
import { ItemStatus } from "../../enums/item-status";

export interface TodoItem {
  id: number;
  title: string;
  description?: string;
  createdAt: string;
  deadline: string;
  priority: ItemPriority;
  remind: boolean;
  status: ItemStatus;
  toDoListID: number;
}
