export interface TodoItemCreate{
  title: string,
  description?: string,
  createdAt: string,
  deadline: string,
  priority: 0 | 1 | 2,
  remind: boolean,
  status: 0 | 1 | 2,
  toDoListID: number
}
