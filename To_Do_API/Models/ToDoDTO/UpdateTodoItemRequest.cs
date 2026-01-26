namespace To_Do_API.Models.ToDoDTO
{
    public class UpdateTodoItemRequest
    {
        public string Name
        {
            get; set;
        }

        public bool IsComplete {  get; set; }
    }
}
