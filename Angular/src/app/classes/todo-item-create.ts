export interface TodoItemCreate{
  title: string,
  description?: string,
  createdAt: string,
  deadline: string,
  priority: number,
  remind: boolean,
  status: number,
  toDoListID: number
}
